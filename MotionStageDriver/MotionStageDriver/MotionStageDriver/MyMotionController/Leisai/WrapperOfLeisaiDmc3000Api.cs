using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;

using CommonApi.MyUtility;

using NLog;

namespace MyMotionStageDriver.MyMotionController.Leisai {
    internal sealed class WrapperOfLeisaiDmc3000Api {
        internal WrapperOfLeisaiDmc3000Api(Dictionary<ushort, Dictionary<ushort, object>> cardChannelMutexes) {
            this.cardChannelMutexes = cardChannelMutexes;

            var cardNoSet = cardChannelMutexes.Keys.ToHashSet();
            foreach (ushort cardNo in cardNoSet) {
                Dictionary<ushort, object> ioMutexDict = new Dictionary<ushort, object>();
                object mutex = new object();
                for (ushort i = 0; i < MAX_IO_CHANNEL_NUMBER; i++) {
                    ioMutexDict.Add(i, mutex);
                }
                cardIoMutexes.Add(cardNo, ioMutexDict);
            }
        }

        static internal (bool isOk, short cardNumber) Initialize() {
            if (initialized) {
                return (true, cardNumber);
            }

            short result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_board_init();
            if (result == 0) {
                LOGGER.Error($"Leisai DMC3000 card does not exist or is abnormal!");
                return (false, result);
            } else if (result < 0) {
                int duplicatedCardNo = Math.Abs(result) - 1;
                LOGGER.Error($"Leisai DMC3000 cardNo(={duplicatedCardNo}) is duplicated!");
                return (false, result);
            } else {
                initialized = true;
                cardNumber = result;
                return (true, result);
            }
        }

        static internal bool Close() {
            short result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_board_close();
            if (result != 0) {
                LOGGER.Error($"LeisaiDmc3000.dmc_board_close() returns {result}.");
                return false;
            }

            return true;
        }

        internal int GetPositionInPulseNumber(ushort cardNo, ushort channelId) {
            object mutext = cardChannelMutexes[cardNo][channelId];
            lock (mutext) {
                return ImportingApiFromLeisaiDmc3000DriverDll.dmc_get_position(cardNo, channelId);
            }
        }

        internal bool MoveRelative(ushort cardNo, ushort channelId, int deltaPulseNumber, bool isStageWithGrattingRuler = false) {
            object mutext = cardChannelMutexes[cardNo][channelId];
            short result;
            bool bRet = true;
            lock (mutext) {
                result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_pmove(cardNo, channelId, deltaPulseNumber, (ushort)EnumPositionMode.RELATIVE);
                if ((result == 0) || (result == 22) || (result == 106) || (result == 107)) {
                    if (result != 0) {
                        LOGGER.Warn($"Leisai DMC3000 dmc_pmove return {result}!");
                        return false;
                    }                   
                }
                //if (result != 0) {
                else {
                    bRet = false;
                    string exceptionMessage = $"LeisaiDmc3000.dmc_pmove({cardNo}, {channelId}, {deltaPulseNumber}, {EnumPositionMode.RELATIVE.ToString()}) returns {result}.";
                    throw new Exception(exceptionMessage);                    
                }
                WaitForMotionCompleted(cardNo, channelId, isStageWithGrattingRuler);

                return bRet;
            }
        }

        internal void MoveAbsolute(ushort cardNo, ushort channelId, int absolutePositionInPulseNumber, bool isStageWithGrattingRuler = false) {
            object mutext = cardChannelMutexes[cardNo][channelId];
            short result;
            lock (mutext) {
                result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_pmove(cardNo, channelId, absolutePositionInPulseNumber, (ushort)EnumPositionMode.ABSOLUTE);
                if ((result == 0) || (result == 22) || (result == 106) || (result == 107)) {
                    if (result != 0) {
                        LOGGER.Warn($"Leisai DMC3000 dmc_pmove return {result}!");
                    }
                }
                //if (result != 0) {
                else {
                    string exceptionMessage = $"LeisaiDmc3000.dmc_pmove({cardNo}, {channelId}, {absolutePositionInPulseNumber}, {EnumPositionMode.ABSOLUTE}) returns {result}.";
                    throw new Exception(exceptionMessage);
                }
                WaitForMotionCompleted(cardNo, channelId, isStageWithGrattingRuler);
            }
        }

        internal bool Homing(ushort cardNo, ushort channelId) {
            object mutext = cardChannelMutexes[cardNo][channelId];
            short result;
            string exceptionMessage;
            lock (mutext) {
                result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_home_move(cardNo, channelId);
                if (result != 0) {
                    exceptionMessage = $"LeisaiDmc3000.dmc_home_move({cardNo}, {channelId}) returns {result}.";
                    throw new Exception(exceptionMessage);
                }
                WaitForMotionCompleted(cardNo, channelId);
                result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_position(cardNo, channelId, 0);
                if (result != 0) {
                    exceptionMessage = $"LeisaiDmc3000.dmc_set_position({cardNo}, {channelId}, 0) returns {result}.";
                    throw new Exception(exceptionMessage);
                }
            }

            return true;
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardNo"></param>
        /// <param name="channelId"></param>
        /// <param name="coderReverse">编码器翻转设置：0不翻转，1翻转</param>
        /// <param name="sevonLevel">伺服有效电平</param>
        /// <param name="limitPos">负限位移动距离 脉冲数</param>
        /// <param name="homeOffset">回零偏移量 脉冲数</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        internal bool Homing_XH(ushort cardNo, ushort channelId, ushort coderReverse, ushort sevonLevel, int limitPos, int homeOffset,double minVel,double maxVel)
        {
            object mutext = cardChannelMutexes[cardNo][channelId];
            short result;
            string exceptionMessage;
            double startVel = 0;
            double stopVel = 0;
            double startAcc = 0;
            double stopAcc = 0;
            double runVel = 0;
            
            lock (mutext) {                
                //编码器反馈值反向设定
                result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_encoder_dir(cardNo, channelId, coderReverse);
                if (result != 0)
                {
                    exceptionMessage = $"LeisaiDmc3000.dmc_set_encoder_dir({cardNo}, {channelId},{coderReverse}) returns {result}.";
                    throw new Exception(exceptionMessage);
                }

                result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_s_profile(cardNo, channelId, 0,0.1);
                if (result != 0) {
                    exceptionMessage = $"LeisaiDmc3000.dmc_set_s_profile({cardNo}, {channelId},{0.1}) returns {result}.";
                    throw new Exception(exceptionMessage);
                }

                //读取当前速度配置
                result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_get_profile(cardNo, channelId, ref startVel, ref runVel, ref startAcc, ref stopAcc, ref stopVel);
                if (result != 0) {
                    exceptionMessage = $"LeisaiDmc3000.dmc_get_profile({cardNo}, {channelId}) returns {result}.";
                    throw new Exception(exceptionMessage);
                }

                //设置速度
                result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_profile(cardNo, channelId, maxVel / 2, maxVel, 0.1, 0.1, maxVel/2);
                if (result != 0) {
                    exceptionMessage = $"LeisaiDmc3000.dmc_set_profile({cardNo}, {channelId}) returns {result}.";
                    throw new Exception(exceptionMessage);
                }

                //关闭告警
                result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_alm_mode(cardNo, channelId, 0,0,0);
                if (result != 0)
                {
                    exceptionMessage = $"LeisaiDmc3000.dmc_set_alm_mode({cardNo}, {channelId}, 0,0,0) returns {result}.";
                    throw new Exception(exceptionMessage);
                }
                //设置伺服使能电平
                result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_write_sevon_pin(cardNo, channelId, sevonLevel);
                if (result != 0)
                {
                    exceptionMessage = $"LeisaiDmc3000.dmc_write_sevon_pin({cardNo}, {channelId},{sevonLevel}) returns {result}.";
                    throw new Exception(exceptionMessage);
                }
                //设置异常停止时减速时间
                result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_dec_stop_time(cardNo, channelId, 0.25);
                if (result != 0)
                {
                    exceptionMessage = $"LeisaiDmc3000.dmc_set_dec_stop_time({cardNo}, {channelId},0.25) returns {result}.";
                    throw new Exception(exceptionMessage);
                }
                //相对运动到负限位处
                result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_pmove(cardNo, channelId, limitPos, 0);
                //if (result != 0)
                if (result == 0 || result == 22) {
                    if (result == 22) {
                        LOGGER.Warn($"Leisai DMC3000 dmc_pmove return 22!");
                    }                    
                }
                else
                {
                    exceptionMessage = $"LeisaiDmc3000.dmc_pmove({cardNo}, {channelId},{limitPos},0) returns {result}.";
                    throw new Exception(exceptionMessage);
                }
                //等待运动到负限位处 
                WaitForMotionCompleted(cardNo, channelId);
                Thread.Sleep(3000);
                //设定值清零
                result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_position(cardNo, channelId, 0);
                if (result != 0) {
                    exceptionMessage = $"LeisaiDmc3000.dmc_set_position({cardNo}, {channelId}, 0) returns {result}.";
                    throw new Exception(exceptionMessage);
                }
                //反馈值值清零
                result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_encoder(cardNo, channelId, 0);
                if (result != 0)
                {
                    exceptionMessage = $"LeisaiDmc3000.dmc_set_encoder({cardNo}, {channelId}, 0) returns {result}.";
                    throw new Exception(exceptionMessage);
                }
#if false
                result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_home_position((ushort)cardNo, (ushort)channelId, 0, homeOffset);//设置偏移模式
                if (result != 0) {
                    exceptionMessage = $"LeisaiDmc3000.dmc_set_home_position({cardNo}, {channelId}) returns {result}.";
                    throw new Exception(exceptionMessage);
                }
#endif
                //关闭告警
                result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_alm_mode(cardNo, channelId, 0, 0, 0);
                if (result != 0) {
                    exceptionMessage = $"LeisaiDmc3000.dmc_set_alm_mode({cardNo}, {channelId}, 0,0,0) returns {result}.";
                    throw new Exception(exceptionMessage);
                }
                //设置伺服使能电平
                result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_write_sevon_pin(cardNo, channelId, sevonLevel);
                if (result != 0) {
                    exceptionMessage = $"LeisaiDmc3000.dmc_write_sevon_pin({cardNo}, {channelId},{sevonLevel}) returns {result}.";
                    throw new Exception(exceptionMessage);
                }

                result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_home_position((ushort)cardNo, (ushort)channelId, 0, 0);//设置偏移模式
                if (result != 0) {
                    exceptionMessage = $"LeisaiDmc3000.dmc_set_home_position({cardNo}, {channelId}) returns {result}.";
                    throw new Exception(exceptionMessage);
                }

                //启动EZ回零
                result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_home_move(cardNo, channelId);//启动回零
                if (result != 0)
                {
                    exceptionMessage = $"LeisaiDmc3000.dmc_home_move({cardNo}, {channelId}) returns {result}.";
                    throw new Exception(exceptionMessage);
                }
                WaitForMotionCompleted(cardNo, channelId);
                Thread.Sleep(3000);

                //设定值清零
                result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_position(cardNo, channelId, 0);
                if (result != 0) {
                    exceptionMessage = $"LeisaiDmc3000.dmc_set_position({cardNo}, {channelId}, 0) returns {result}.";
                    throw new Exception(exceptionMessage);
                }
                //反馈值值清零
                result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_encoder(cardNo, channelId, 0);
                if (result != 0) {
                    exceptionMessage = $"LeisaiDmc3000.dmc_set_encoder({cardNo}, {channelId}, 0) returns {result}.";
                    throw new Exception(exceptionMessage);
                }

#if true
                //相对运动到负限位处
                result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_pmove(cardNo, channelId, homeOffset, 0);
                //if (result != 0)
                if (result == 0 || result == 22) {
                    if (result == 22) {
                        LOGGER.Warn($"Leisai DMC3000 dmc_pmove return 22!");
                    }
                } else {
                    exceptionMessage = $"LeisaiDmc3000.dmc_pmove({cardNo}, {channelId},{limitPos},0) returns {result}.";
                    throw new Exception(exceptionMessage);
                }
                //等待运动到负限位处 
                WaitForMotionCompleted(cardNo, channelId);
                Thread.Sleep(3000);
                //设定值清零
                result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_position(cardNo, channelId, 0);
                if (result != 0) {
                    exceptionMessage = $"LeisaiDmc3000.dmc_set_position({cardNo}, {channelId}, 0) returns {result}.";
                    throw new Exception(exceptionMessage);
                }
                //反馈值值清零
                result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_encoder(cardNo, channelId, 0);
                if (result != 0) {
                    exceptionMessage = $"LeisaiDmc3000.dmc_set_encoder({cardNo}, {channelId}, 0) returns {result}.";
                    throw new Exception(exceptionMessage);
                }
#endif

            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardNo"></param>
        /// <param name="channelId"></param>
        /// <param name="coderReverse">编码器翻转设置：0不翻转，1翻转</param>
        /// <param name="sevonLevel">伺服有效电平</param>
        /// <param name="limitPos">负限位移动距离 脉冲数</param>
        /// <param name="homeOffset">回零偏移量 脉冲数</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        internal bool Homing_XH_Z(ushort cardNo, ushort channelId, ushort coderReverse, ushort sevonLevel, int limitPos, int homeOffset, double minVel, double maxVel) {
            object mutext = cardChannelMutexes[cardNo][channelId];
            short result;
            string exceptionMessage;
            double startVel = 0;
            double stopVel = 0;
            double startAcc = 0;
            double stopAcc = 0;
            double runVel = 0;

            lock (mutext) {
                //编码器反馈值反向设定
                result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_encoder_dir(cardNo, channelId, coderReverse);
                if (result != 0) {
                    exceptionMessage = $"LeisaiDmc3000.dmc_set_encoder_dir({cardNo}, {channelId},{coderReverse}) returns {result}.";
                    throw new Exception(exceptionMessage);
                }

                result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_s_profile(cardNo, channelId, 0, 0.2);
                if (result != 0) {
                    exceptionMessage = $"LeisaiDmc3000.dmc_set_s_profile({cardNo}, {channelId},{0.1}) returns {result}.";
                    throw new Exception(exceptionMessage);
                }

                //读取当前速度配置
                result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_get_profile(cardNo, channelId, ref startVel, ref runVel, ref startAcc, ref stopAcc, ref stopVel);
                if (result != 0) {
                    exceptionMessage = $"LeisaiDmc3000.dmc_get_profile({cardNo}, {channelId}) returns {result}.";
                    throw new Exception(exceptionMessage);
                }

                //设置速度
                result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_profile(cardNo, channelId, minVel, maxVel, 0.05, 0.05, maxVel / 2);
                if (result != 0) {
                    exceptionMessage = $"LeisaiDmc3000.dmc_set_profile({cardNo}, {channelId}) returns {result}.";
                    throw new Exception(exceptionMessage);
                }

                //关闭告警
                result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_alm_mode(cardNo, channelId, 0, 0, 0);
                if (result != 0) {
                    exceptionMessage = $"LeisaiDmc3000.dmc_set_alm_mode({cardNo}, {channelId}, 0,0,0) returns {result}.";
                    throw new Exception(exceptionMessage);
                }
                //设置伺服使能电平
                result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_write_sevon_pin(cardNo, channelId, sevonLevel);
                if (result != 0) {
                    exceptionMessage = $"LeisaiDmc3000.dmc_write_sevon_pin({cardNo}, {channelId},{sevonLevel}) returns {result}.";
                    throw new Exception(exceptionMessage);
                }
                //设置异常停止时减速时间
                result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_dec_stop_time(cardNo, channelId, 0.25);
                if (result != 0) {
                    exceptionMessage = $"LeisaiDmc3000.dmc_set_dec_stop_time({cardNo}, {channelId},0.25) returns {result}.";
                    throw new Exception(exceptionMessage);
                }
                
                result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_home_position((ushort)cardNo, (ushort)channelId, 0, homeOffset);//设置偏移模式
                if (result != 0) {
                    exceptionMessage = $"LeisaiDmc3000.dmc_set_home_position({cardNo}, {channelId}) returns {result}.";
                    throw new Exception(exceptionMessage);
                }

                //关闭告警
                result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_alm_mode(cardNo, channelId, 0, 0, 0);
                if (result != 0) {
                    exceptionMessage = $"LeisaiDmc3000.dmc_set_alm_mode({cardNo}, {channelId}, 0,0,0) returns {result}.";
                    throw new Exception(exceptionMessage);
                }
                //设置伺服使能电平
                result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_write_sevon_pin(cardNo, channelId, sevonLevel);
                if (result != 0) {
                    exceptionMessage = $"LeisaiDmc3000.dmc_write_sevon_pin({cardNo}, {channelId},{sevonLevel}) returns {result}.";
                    throw new Exception(exceptionMessage);
                }

                result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_home_position((ushort)cardNo, (ushort)channelId, 0, homeOffset);//设置偏移模式
                if (result != 0) {
                    exceptionMessage = $"LeisaiDmc3000.dmc_set_home_position({cardNo}, {channelId}) returns {result}.";
                    throw new Exception(exceptionMessage);
                }

                //启动回零
                result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_home_move(cardNo, channelId);//启动回零
                if (result != 0) {
                    exceptionMessage = $"LeisaiDmc3000.dmc_home_move({cardNo}, {channelId}) returns {result}.";
                    throw new Exception(exceptionMessage);
                }
                WaitForMotionCompleted(cardNo, channelId);
                Thread.Sleep(3000);

                //设定值清零
                result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_position(cardNo, channelId, 0);
                if (result != 0) {
                    exceptionMessage = $"LeisaiDmc3000.dmc_set_position({cardNo}, {channelId}, 0) returns {result}.";
                    throw new Exception(exceptionMessage);
                }
                //反馈值值清零
                result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_encoder(cardNo, channelId, 0);
                if (result != 0) {
                    exceptionMessage = $"LeisaiDmc3000.dmc_set_encoder({cardNo}, {channelId}, 0) returns {result}.";
                    throw new Exception(exceptionMessage);
                }           
            }

            return true;
        }

        internal void SetLeadcrewCompConfig(ushort cardNo, ushort channelId, ushort numPos, int posStart, int posDis, int[] compPos, int[] compNeg)
        {
            short result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_leadscrew_comp_datasheet_enable(cardNo, channelId, 1, 1000);
            if (result != 0)
            {
                string exceptionMessage = $"LeisaiDmc3000.dmc_set_leadscrew_comp_datasheet_enable({cardNo}, {channelId}) returns {result}!";
                throw new Exception(exceptionMessage);                
            }

            result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_leadscrew_comp_config(cardNo, channelId, numPos, posStart, posDis, compPos, compNeg);
            if (result != 0)
            {
                string exceptionMessage = $"LeisaiDmc3000.dmc_set_leadscrew_comp_config({cardNo}, {channelId}, {numPos}, {posStart}, {posDis}) returns {result}!";
                throw new Exception(exceptionMessage);
            }
        }

        internal void EnableLeadcrewComp(ushort cardNo, ushort channelId, ushort enable)
        {
            short result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_enable_leadscrew_comp(cardNo, channelId, enable);
            if (result != 0)
            {
                string exceptionMessage = $"LeisaiDmc3000.dmc_enable_leadscrew_comp({cardNo}, {channelId}, {enable}) returns {result}!";
                throw new Exception(exceptionMessage);
            }
        }

        internal void ClearEncoderPos(ushort cardNo, ushort channelId) {
            short result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_encoder(cardNo, channelId, 0);
            if (result != 0) {
                string exceptionMessage = $"LeisaiDmc3000.dmc_set_encoder({cardNo}, {channelId}) returns {result}!";
                throw new Exception(exceptionMessage);
            }
        }

        internal void SetHomeOffset(ushort cardNo, ushort channelId, double homeOffset) {
            short result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_home_position(cardNo, channelId, 0, homeOffset);
            if (result != 0) {
                string exceptionMessage = $"LeisaiDmc3000.dmc_set_home_position({cardNo}, {channelId}, {homeOffset}) returns {result}!";
                throw new Exception(exceptionMessage);
            }
        }

        internal void SetSoftLimit(ushort cardNo, ushort channelId, ushort enable, ushort source, ushort action, int negLimit, int posLimit)
        {
            short result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_softlimit(cardNo, channelId, enable, source, action, negLimit, posLimit);
            if (result != 0)
            {
                string exceptionMessage = $"LeisaiDmc3000.dmc_set_softlimit({cardNo}, {channelId}, {enable},{source}, {action}, {negLimit}, {posLimit}) returns {result}!";
                throw new Exception(exceptionMessage);
            }
        }

        internal void SetEncoderDir(ushort cardNo, ushort channelId, ushort dir) {
            short result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_encoder_dir(cardNo, channelId, dir);
            if (result != 0) {
                string exceptionMessage = $"LeisaiDmc3000.dmc_set_encoder_dir({cardNo}, {channelId}, {dir}) returns {result}!";
                throw new Exception(exceptionMessage);
            }
        }

        internal void WaitForMotionCompleted(ushort cardNo, ushort channelId,bool isStageWithGrattingRuler = false) {            
            int prePos = 0;
            int curPos = 0;
            DateTime timeStart = DateTime.Now;  
            while (true) {               
                Thread.Sleep(TimeSpan.FromMilliseconds(INTERVAL_IN_MS_QUERYING_IF_MOTIONLESS));
                short result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_check_done(cardNo, channelId);
                if (result == (short)EnumChannelMotionStatus.MOTIONLESS) {
                    //result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_check_success_encoder(cardNo, channelId);
                    if (isStageWithGrattingRuler) {
                        curPos = ImportingApiFromLeisaiDmc3000DriverDll.dmc_get_encoder(cardNo, channelId);
                        if (curPos == prePos) {
                            return;
                        }  else {
                            Thread.Sleep(TimeSpan.FromMilliseconds(4 * INTERVAL_IN_MS_QUERYING_IF_MOTIONLESS));
                            prePos = curPos;
                        }
                    }  else {
                        return;
                    }                                                          
                }

                DateTime timeNow= DateTime.Now;
                TimeSpan span = timeNow - timeStart;

                if (span.TotalSeconds > 300)
                {
                    throw new Exception($"Card {cardNo} Channel {channelId} WaitForMotionCompleted Timeout");
                }
            }
        }

        internal void ChangeSpeed(ushort cardNo, ushort channelId, int speedInPps) {
            short result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_change_speed(cardNo, channelId, speedInPps, 0.1);
            if (result != 0) {
                string exceptionMessage = $"LeisaiDmc3000.dmc_change_speed({cardNo}, {channelId}, {speedInPps}, 0) returns {result}!";
                throw new Exception(exceptionMessage);
            }
        }

        internal int GetSpeed(ushort cardNo, ushort channelId) {
            double speed = ImportingApiFromLeisaiDmc3000DriverDll.dmc_read_current_speed(cardNo, channelId);
            return (int)Math.Floor(speed);
        }

        internal void Stop(ushort cardNo, ushort channelId) {
            short result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_stop(cardNo, channelId, (ushort)EnumStopMode.SLOW_DOWN_STOP);
            if (result != 0) {
                string exceptionMessage = $"LeisaiDmc3000.dmc_stop({cardNo}, {channelId}, {(ushort)EnumStopMode.SLOW_DOWN_STOP}) returns {result}!";
                throw new Exception(exceptionMessage);
            }
        }

        internal void EmergencyStop(ushort cardNo, ushort channelId) {
            short result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_stop(cardNo, channelId, (ushort)EnumStopMode.EMERGENCY_STOP);
            if (result != 0) {
                string exceptionMessage = $"LeisaiDmc3000.dmc_stop({cardNo}, {channelId}, {(ushort)EnumStopMode.EMERGENCY_STOP}) returns {result}!";
                throw new Exception(exceptionMessage);
            }
        }

        internal void EmergencyStopAllChannelsAtCard(ushort cardNo) {
            short result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_emg_stop(cardNo);
            if (result != 0) {
                string exceptionMessage = $"LeisaiDmc3000.dmc_emg_stop({cardNo}) returns {result}!";
                throw new Exception(exceptionMessage);
            }
        }

        internal void HcmpSetConfig(ushort cardNo, ushort channelId, ushort hcmp, ushort cmpSource, ushort cmpLogic, int time) {
            short result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_hcmp_set_config(cardNo, hcmp, channelId, cmpSource, cmpLogic, time);
            if (result != 0) {
                string exceptionMessage = $"LeisaiDmc3000.dmc_hcmp_set_config({cardNo}, {hcmp}, {channelId}, {cmpSource}, {cmpLogic}, {time}) returns {result}!";
                throw new Exception(exceptionMessage);
            }
        }

        internal void HcmpSetMode(ushort cardNo, ushort hcmp, ushort cmpEnable) {
            short result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_hcmp_set_mode(cardNo, hcmp, cmpEnable);
            if (result != 0) {
                string exceptionMessage = $"LeisaiDmc3000.dmc_hcmp_set_mode({cardNo}, {hcmp}, {cmpEnable}) returns {result}!";
                throw new Exception(exceptionMessage);
            }
        }

        internal void HcmpClearPoints(ushort cardNo, ushort hcmp) {
            short result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_hcmp_clear_points(cardNo, hcmp);
            if (result != 0) {
                string exceptionMessage = $"LeisaiDmc3000.dmc_hcmp_clear_points({cardNo}, {hcmp}) returns {result}!";
                throw new Exception(exceptionMessage);
            }
        }

        internal void HcmpSetLiner(ushort cardNo, ushort hcmp, int increment, int count) {
            short result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_hcmp_set_liner(cardNo, hcmp, increment, count);
            if (result != 0) {
                string exceptionMessage = $"LeisaiDmc3000.dmc_hcmp_set_liner({cardNo}, {hcmp}, {increment}, {count}) returns {result}!";
                throw new Exception(exceptionMessage);
            }
        }

        internal void HcmpAddPoint(ushort cardNo, ushort hcmp, int cmpPos) {
            short result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_hcmp_add_point(cardNo, hcmp, cmpPos);
            if (result != 0) {
                string exceptionMessage = $"LeisaiDmc3000.dmc_hcmp_add_point({cardNo}, {hcmp}, {cmpPos}) returns {result}!";
                throw new Exception(exceptionMessage);
            }
        }

        internal short ReadIoInput(ushort cardNo, ushort ioId) {
            object mutext = cardIoMutexes[cardNo][ioId];
            lock (mutext) {
                return ImportingApiFromLeisaiDmc3000DriverDll.dmc_read_inbit(cardNo, ioId);
            }
        }

        internal void WriteIoOutput(ushort cardNo, ushort ioId, ushort output) {
            object mutext = cardIoMutexes[cardNo][ioId];
            lock (mutext) {
                short result = ImportingApiFromLeisaiDmc3000DriverDll.dmc_write_outbit(cardNo, ioId, output);
                if (result != 0) {
                    string exceptionMessage = $"LeisaiDmc3000.dmc_write_outbit({cardNo}, {ioId}, {output}) returns {result}!";
                    throw new Exception(exceptionMessage);
                }
            }
        }

        internal short ReadIoOutput(ushort cardNo, ushort ioId) {
            object mutext = cardIoMutexes[cardNo][ioId];
            lock (mutext) {
                return ImportingApiFromLeisaiDmc3000DriverDll.dmc_read_outbit(cardNo, ioId);
            }
        }

        internal short SetChannelIoMap(ushort cardNo, ushort channelId, ushort ioType, ushort mapIoType, ushort mapIoIndex, double filter) {
            return ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_axis_io_map(cardNo, channelId, ioType, mapIoType, mapIoIndex, filter);
        }

        internal short SetEmgMode(ushort cardNo, ushort channelId, ushort enable, ushort emgLogic) {
            return ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_emg_mode(cardNo, channelId, enable, emgLogic);
        }

        internal short SetAxisSpeed(ushort cardNo, ushort channelId, double minVel, double maxVel,double acc) {
            //return ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_profile(cardNo, channelId, maxVel / 2, maxVel, acc, acc, maxVel/2);            
            return ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_profile(cardNo, channelId, maxVel / 2, maxVel, acc, acc, maxVel);
        }

        internal void GetAxisSpeed(ushort cardNo, ushort channelId, ref double minVel, ref double maxVel, ref double acc) {
            double tacc = 0;
            double stopVel = 0;
            ImportingApiFromLeisaiDmc3000DriverDll.dmc_get_profile(cardNo, channelId, ref minVel, ref maxVel, ref acc, ref tacc, ref stopVel);
        }

        internal short SetAxisSpeedEx(ushort cardNo, ushort channelId, double minVel, double maxVel, double acc) {
            return ImportingApiFromLeisaiDmc3000DriverDll.dmc_set_profile(cardNo, channelId, minVel, maxVel, acc, acc, 0);
        }

        private static bool initialized = false;
        private static short cardNumber = 0;
        private static readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
        private readonly Dictionary<ushort, Dictionary<ushort, object>> cardChannelMutexes = new Dictionary<ushort, Dictionary<ushort, object>>();
        private readonly Dictionary<ushort, Dictionary<ushort, object>> cardIoMutexes = new Dictionary<ushort, Dictionary<ushort, object>>();
        private const int INTERVAL_IN_MS_QUERYING_IF_MOTIONLESS = 10;
        private const int MAX_IO_CHANNEL_NUMBER = 16;

        private enum EnumPositionMode {
            RELATIVE = 0,
            ABSOLUTE
        }
        private enum EnumChannelMotionStatus {
            MOVING,
            MOTIONLESS
        }
        private enum EnumStopMode {
            SLOW_DOWN_STOP = 0,
            EMERGENCY_STOP
        }
    }
}

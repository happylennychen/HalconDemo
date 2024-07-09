using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

using CommonApi.MyEnum;
using CommonApi.MyUtility;

using NLog;

using ProberApi.MyException;

namespace ProberApi.MyBoard {
    public sealed class RequestStatusBoard : ConcurrentDictionary<string, (DateTime receivedTimestamp, string requestType, EnumRequestStatus requestStatus, string requestResult, DateTime completedTimestamp)> {
        public RequestStatusBoard() {
            //[gyh]刷新间隔，放到配置文件中
            timerRemoveOutdatedRecords.Interval = 10 * 1000;
            timerRemoveOutdatedRecords.Elapsed += TimerRemoveOutdatedRecords_Elapsed;
            timerRemoveOutdatedRecords.AutoReset = true;
            timerRemoveOutdatedRecords.Enabled = true;
        }

        public string Query(string requestSerialNum) {
            lock (mutex) {
                if (!this.ContainsKey(requestSerialNum)) {
                    LOGGER.Error($"Request serial num(={requestSerialNum}) does not exist!");
                    return $"{EnumResponseId.INVALID_REQUEST_SERIAL_NUMBER},,,";
                }

                var request = this[requestSerialNum];
                if (request.requestStatus >= EnumRequestStatus.COMPLETED_PASS) {
                    this.TryRemove(requestSerialNum, out var temp);
                }
                return $"{EnumResponseId.PASS},{request.requestType},{request.requestStatus},{request.requestResult}";
            }
        }

        public void AddNew(string requestSerialNum, string requestType) {
            lock (mutex) {
                (DateTime receivedTimestamp, string requestType, EnumRequestStatus requestStatus, string requestResult, DateTime completedTimestamp) newValue
                    = (DateTime.Now, requestType, EnumRequestStatus.READY, string.Empty, DateTime.MinValue);
                this.AddOrUpdate(requestSerialNum, newValue, (key, oldValue) => newValue);
            }
        }

        public void UpdateBeginRunning(string requestSerialNum) {
            lock (mutex) {
                if (!this.ContainsKey(requestSerialNum)) {
                    throw new RequestSerialNumberDoesNotExistException(requestSerialNum);
                }

                var lastValue = this[requestSerialNum];
                (DateTime receivedTimestamp, string requestType, EnumRequestStatus requestStatus, string requestResult, DateTime completedTimestamp) newValue
                    = (lastValue.receivedTimestamp, lastValue.requestType, EnumRequestStatus.RUNNING, string.Empty, DateTime.MaxValue);
                this.AddOrUpdate(requestSerialNum, newValue, (key, oldValue) => newValue);
            }
        }

        public void UpdateCompleted(string requestSerialNum, EnumRequestStatus newStatus, string requestResult) {
            lock (mutex) {
                if (!this.ContainsKey(requestSerialNum)) {
                    throw new RequestSerialNumberDoesNotExistException(requestSerialNum);
                }

                var lastValue = this[requestSerialNum];
                (DateTime receivedTimestamp, string requestType, EnumRequestStatus requestStatus, string requestResult, DateTime completedTimestamp)
                    newValue = (lastValue.receivedTimestamp, lastValue.requestType, newStatus, requestResult, DateTime.Now);

                this.AddOrUpdate(requestSerialNum, newValue, (key, oldValue) => newValue);
            }
        }

        public bool IsCompleted(string requestSerialNum) {
            lock (mutex) {
                if (!this.ContainsKey(requestSerialNum)) {
                    throw new RequestSerialNumberDoesNotExistException(requestSerialNum);
                }

                var lastValue = this[requestSerialNum];
                return lastValue.requestStatus >= EnumRequestStatus.COMPLETED_PASS;
            }
        }

        public void RemoveItem(string requestSerialNum) {
            lock (mutex) {
                if (!this.ContainsKey(requestSerialNum)) {
                    throw new RequestSerialNumberDoesNotExistException(requestSerialNum);
                }

                this.TryRemove(requestSerialNum, out var temp);                                
            }
        }

        public string GetText() {
            lock (mutex) {
                List<string> list = new List<string>();
                foreach (var one in this) {
                    string requestSerialNumber = one.Key;
                    var requestStatus = one.Value;
                    string item = $"serial number={requestSerialNumber}, type={requestStatus.requestType}, status={requestStatus.requestStatus}, result={requestStatus.requestResult}";
                    list.Add(item);
                }
                list.Sort();

                StringBuilder result = new StringBuilder();
                foreach (string one in list) {
                    result.AppendLine(one);
                }

                return result.ToString();
            }
        }

        private void TimerRemoveOutdatedRecords_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            lock (mutex) {
                List<string> outdatedSerialNumberList = new List<string>();
                foreach (var one in this) {
                    string serialNumber = one.Key;
                    var value = one.Value;
                    if (value.requestStatus < EnumRequestStatus.COMPLETED_PASS) {
                        continue;
                    }

                    TimeSpan timeSpan = DateTime.Now - value.completedTimestamp;
                    //[gyh]: 过时时长，放到配置文件中？？？
                    if (timeSpan.Days > 0 || timeSpan.Hours > 0 || timeSpan.Minutes > 10) {
                        outdatedSerialNumberList.Add(serialNumber);
                    }
                }

                foreach (string serialNumber in outdatedSerialNumberList) {
                    this.TryRemove(serialNumber, out var temp);
                }
            }
        }

        private static readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
        private readonly System.Timers.Timer timerRemoveOutdatedRecords = new System.Timers.Timer();
        private readonly object mutex = new object();
    }
}

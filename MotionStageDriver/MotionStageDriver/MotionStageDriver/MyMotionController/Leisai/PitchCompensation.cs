using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YTN_CALIBRATION;


namespace MyMotionStageDriver.MyMotionController.Leisai {
    public class PitchCompensation
    {
        private string AxisName { get; set; } = string.Empty;
        public double StartPosition { get; set; }
        public double StopPosition { get; set; }
        public List<PitchCompensationInfo> CompensateInfos { get; set; } = new List<PitchCompensationInfo>();
        public PitchCompensation(string name)
        {
            AxisName = name;
        }

        public PitchCompensation() {

        }

        public bool GenerateCompensateInfo(double start, double stop, double step, double[] positive, double[] negtive)
        {
            CompensateInfos.Clear();
            StartPosition = start;
            StopPosition = stop;
            negtive = negtive.Reverse().ToArray();
            for (int i = 0; i < positive.Length; i++)
            {
                PitchCompensationInfo info = new PitchCompensationInfo();
                info.SetPosition = start + i * step;
                info.PostiveRelPosition = info.SetPosition + positive[i];
                info.NegtiveRelPosition = info.SetPosition + negtive[i];
                CompensateInfos.Add(info);
            }
            return true;
        }

        public double GetSendSetPosition(double relPosition, bool isPos)
        {
            double setPos = -1;
            if (isPos)
            {
                var second = CompensateInfos.First(t => t.PostiveRelPosition > relPosition);
                var index = CompensateInfos.IndexOf(second);
                var first = CompensateInfos[index - 1];
                NumericalCal.LineInterpolation(new double[] { first.PostiveRelPosition, second.PostiveRelPosition }, new double[] { first.SetPosition, second.SetPosition }, relPosition, out setPos);
            }
            else
            {
                var second = CompensateInfos.First(t => t.PostiveRelPosition > relPosition);
                var index = CompensateInfos.IndexOf(second);
                var first = CompensateInfos[index - 1];
                NumericalCal.LineInterpolation(new double[] { first.NegtiveRelPosition, second.NegtiveRelPosition }, new double[] { first.SetPosition, second.SetPosition }, relPosition, out setPos);
            }
            return Math.Round(setPos, 1, MidpointRounding.AwayFromZero);
        }



        public double GetRelPosition(double setPosition, bool isPos)
        {
            var second = CompensateInfos.First(t => t.SetPosition > setPosition);
            var index = CompensateInfos.IndexOf(second);
            var first = CompensateInfos[index - 1];
            double relPos = -1;
            if (isPos)
            {
                NumericalCal.LineInterpolation(new double[] { first.SetPosition, second.SetPosition }, new double[] { first.PostiveRelPosition, second.PostiveRelPosition }, setPosition, out relPos);

            }
            else
            {
                NumericalCal.LineInterpolation(new double[] { first.SetPosition, second.SetPosition }, new double[] { first.NegtiveRelPosition, second.NegtiveRelPosition }, setPosition, out relPos);
            }
            return Math.Round(relPos, 1, MidpointRounding.AwayFromZero);
        }
    }

    public class PitchCompensationInfo
    {
        public double SetPosition { get; set; }
        public double PostiveRelPosition { get; set; }
        public double NegtiveRelPosition { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YTN_CALIBRATION
{
    public class CenterOfRotationCalibration
    {

        public CenterOfRotationCalibration()
        {

        }

        private List<double> _ps_X { get; set; } = new List<double>();
        private List<double> _ps_Y { get; set; } = new List<double>();

        public void AddPoint(double x, double y)
        {
            _ps_X.Add(x);
            _ps_Y.Add(y);
        }

        public bool GetCenterPos(out double centerX, out double centerY)
        {
            centerX = 0;
            centerY = 0;
            if (_ps_Y.Count != _ps_X.Count)
            {
                return false;
            }
            if (_ps_X.Count < 3 || _ps_Y.Count < 3)
            {
                return false;
            }
            

            return true;

            
        }

    }

}

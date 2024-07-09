namespace Prober.WaferDef {
    public class DoubleLIneInterpolation
    {

        public static bool GetHeight(DLInterporationInfo info, double x0, double y0, out double z0)
        {
            z0 = 0;
            if (!NumericalCal.LineInterpolation(new double[] { info.LeftTop.X, info.RightTop.X }, new double[] { info.LeftTop.Z, info.RightTop.Z }, x0, out double z10))
                return false;
            if (!NumericalCal.LineInterpolation(new double[] { info.LeftDown.X, info.RightDown.X }, new double[] { info.LeftDown.Z, info.RightDown.Z }, x0, out double z20))
                return false;
            return NumericalCal.LineInterpolation(new double[] { info.LeftTop.Y, info.LeftDown.Y }, new double[] { z10, z20 }, y0, out z0);
        }

    }
}

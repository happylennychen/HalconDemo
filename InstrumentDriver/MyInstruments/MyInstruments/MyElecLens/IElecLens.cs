namespace MyInstruments.MyElecLens
{
    public interface IElecLens
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">放大倍数</param>
        /// <returns></returns>
        bool SetZoom(double value);


        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool GetZoom(out double value);

        /// <summary>
        /// 放大运动是否结束
        /// </summary>
        /// <returns>true:结束；false：运动中</returns>
        bool IsMoveComplete();
    }
}

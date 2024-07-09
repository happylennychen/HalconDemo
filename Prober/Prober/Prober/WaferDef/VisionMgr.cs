using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prober.WaferDef
{
    public class VisionMgr
    {
        private static HDevEngine _engine;

        public void StartDebug()
        {
            _engine.StartDebugServer();
        }

        public static bool InitEngine()
        {
            if (_engine == null)
                _engine = new HDevEngine();
            else
                return true;
            _engine.SetProcedurePath(AppDomain.CurrentDomain.BaseDirectory + "\\halconProcedure");
            return true;
        }      

        public static bool CalSX1Method(HObject image, HWindow wind, out double angle)
        {            
            angle = 0;
            bool bRet = false;

            try
            {
                HDevProcedure procedure = new HDevProcedure("SX1Angle");
                HDevProcedureCall call = new HDevProcedureCall(procedure);
                wind.ClearWindow();
                wind.DispObj(image);
                HOperatorSet.Rgb1ToGray(image, out image);
                call.SetInputIconicParamObject("InputImage", image);

                call.Execute();
                HTuple h_angle = call.GetOutputCtrlParamTuple("ResAngle");
                HObject line1 = call.GetOutputIconicParamObject("RLine1");
                HObject line2 = call.GetOutputIconicParamObject("RLine2");
                HOperatorSet.DispObj(line1, wind);
                HOperatorSet.DispObj(line2, wind);
                angle = h_angle.D;

                bRet = true;
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return bRet;
        }


        public static bool GetWaferIdFromImage(HObject image, out string id)
        {
            id = string.Empty;
            bool bRet = false;  

            try
            {
                HDevProcedure procedure = new HDevProcedure("WaferID");
                HDevProcedureCall call = new HDevProcedureCall(procedure);
                HOperatorSet.Rgb1ToGray(image, out image);
                call.SetInputIconicParamObject("InputImage", image);
                call.Execute();
                HTuple h_id = call.GetOutputCtrlParamTuple("ID");
                id = h_id.S;

                bRet = true;
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return bRet;
        }                   

        public static HTuple row1;
        public static HTuple col1;
        public static HTuple row2;
        public static HTuple col2;
        public static bool SelectMark(HTuple _winID, HObject _image, out HTuple _modelID, out HTuple _FRow, out HTuple _FColumn)
        {
            _modelID = new HTuple();
            _FRow = new HTuple();
            _FColumn = new HTuple();
            bool bRet = false;  

            try
            {
                HOperatorSet.Rgb1ToGray(_image, out _image);
                HOperatorSet.DrawRectangle1(_winID, out row1, out col1, out row2, out col2);
                HOperatorSet.GenRectangle1(out HObject rect, row1, col1, row2, col2);
                HOperatorSet.ReduceDomain(_image, rect, out HObject reduced);
                HTuple h = new HTuple(20, 30, 30);

                _modelID?.Dispose();
                HOperatorSet.CreateShapeModel(reduced, "auto", -0.39, 0.79, "auto", "auto", "ignore_local_polarity", h, "auto", out _modelID);

                HOperatorSet.GetShapeModelParams(_modelID, out HTuple numlevels, out HTuple angleStart, out HTuple angleExtent, out HTuple angleStep, out HTuple scalMin, out HTuple scalMax, out HTuple scanStep, out HTuple m, out HTuple minConstarst);
                //HOperatorSet.FindShapeModel()
                int num = numlevels - 2 >= 3 ? numlevels - 2 : numlevels;

                HOperatorSet.FindShapeModel(_image, _modelID, -0.39, 0.79, 0.85, 1, 0.5, "least_squares", num, 0.9, out _FRow, out _FColumn, out HTuple fangle, out HTuple fscore);
                HOperatorSet.GetShapeModelContours(out HObject contours, _modelID, 1);
                HOperatorSet.VectorAngleToRigid(0, 0, 0, _FRow, _FColumn, fangle, out HTuple mat2D);
                HOperatorSet.AffineTransContourXld(contours, out HObject transContours, mat2D);
                HOperatorSet.DispObj(transContours, _winID);
                bRet = true;
            }
            catch (Exception ex)
            {
                throw(ex);
            }

            return bRet;
        }        

        public static bool FindPlatMark(HObject image, HTuple modelID, out double rowRes, out double colRes)
        {
            rowRes = 0;
            colRes = 0;
            HTuple row = new HTuple();
            HTuple col = new HTuple();
            HTuple angle = new HTuple();
            HTuple score = new HTuple();
            bool bRet = false;  

            try
            {
                HOperatorSet.GetShapeModelParams(modelID, out HTuple nums, out HTuple start, out HTuple end, out HTuple step, out HTuple scanmin, out HTuple scalemax, out HTuple scaleStep, out HTuple metric, out HTuple mincontrast);
                HOperatorSet.Rgb1ToGray(image, out image);
                HOperatorSet.FindShapeModel(image, modelID, start, end, 0.8, 1, 0.5, "least_squares", nums - 1, 0.9, out row, out col, out angle, out score);
                if (score.Length < 1)
                {
                    throw new Exception("FindPlatMark() 未找到Mark特征!");
                }
                else if (score.D < 0.8)
                {
                    throw new Exception($"FindPlatMark() 得分为{score.D},太低");                   
                }
                else
                {
                    rowRes = row.D;
                    colRes = col.D;
                    bRet = true;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {

                row.Dispose();
                col.Dispose();
                angle.Dispose();
                score.Dispose();
                image.Dispose();
            }

            return bRet;
        }          

        internal static double GetImageShape(HObject image)
        {
            double amp = 0;

            HOperatorSet.Rgb1ToGray(image, out image);
            HOperatorSet.GetImageSize(image, out HTuple width, out HTuple height);
            HTuple hv_Row1 = 500, hv_Column1 = 500, hv_Row2 = height.D - 500, hv_Column2 = width - 500;
            HOperatorSet.GenRectangle1(out HObject ho_Rectangle, hv_Row1, hv_Column1, hv_Row2, hv_Column2);

            HOperatorSet.ReduceDomain(image, ho_Rectangle, out HObject ho_ImageReduced);
            HOperatorSet.SobelAmp(ho_ImageReduced, out HObject ho_EdgeAmplitude, "sum_abs", 5);
            HOperatorSet.Intensity(ho_Rectangle, ho_EdgeAmplitude, out HTuple hv_Mean, out HTuple hv_Deviation);
            amp = hv_Mean.D;

            return amp;
        }

        public static bool GetAngleBetweenFiberAndPlat(bool isLeft, HObject ho_L1, HTuple hv_WindowHandle, out double angle)
        {
            // Local iconic variables 
            angle = 0;
            HObject ho_GrayImage, ho_Rectangle;
            HObject ho_ImageReduced, ho_Regions, ho_RegionClosing, ho_RegionOpening;
            HObject ho_ConnectedRegions, ho_SortedRegions, ho_ObjectSelected1;
            HObject ho_ObjectSelected2, ho_Skeleton, ho_RegionClipped;
            HObject ho_Contours, ho_RegionBorder, ho_RegionClipped1;
            HObject ho_Skeleton2, ho_Contours2, ho_RegionLines, ho_RegionLines1;

            // Local control variables 
            HTuple hv_Row1 = new HTuple();
            HTuple hv_Column1 = new HTuple(), hv_Row2 = new HTuple();
            HTuple hv_Column2 = new HTuple(), hv_RowBegin = new HTuple();
            HTuple hv_ColBegin = new HTuple(), hv_RowEnd = new HTuple();
            HTuple hv_ColEnd = new HTuple(), hv_Nr = new HTuple();
            HTuple hv_Nc = new HTuple(), hv_Dist = new HTuple(), hv_RowBegin1 = new HTuple();
            HTuple hv_ColBegin1 = new HTuple(), hv_RowEnd1 = new HTuple();
            HTuple hv_ColEnd1 = new HTuple(), hv_Nr1 = new HTuple();
            HTuple hv_Nc1 = new HTuple(), hv_Dist1 = new HTuple();
            HTuple hv_Angle = new HTuple(), hv_Deg = new HTuple();
            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_IsOverlapping = new HTuple();

            bool bRet = false;  

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_GrayImage);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SortedRegions);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected1);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected2);
            HOperatorSet.GenEmptyObj(out ho_Skeleton);
            HOperatorSet.GenEmptyObj(out ho_RegionClipped);
            HOperatorSet.GenEmptyObj(out ho_Contours);
            HOperatorSet.GenEmptyObj(out ho_RegionBorder);
            HOperatorSet.GenEmptyObj(out ho_RegionClipped1);
            HOperatorSet.GenEmptyObj(out ho_Skeleton2);
            HOperatorSet.GenEmptyObj(out ho_Contours2);
            HOperatorSet.GenEmptyObj(out ho_RegionLines);
            HOperatorSet.GenEmptyObj(out ho_RegionLines1);

            try
            {
                ho_GrayImage.Dispose();
                HOperatorSet.Rgb1ToGray(ho_L1, out ho_GrayImage);

                hv_Row1.Dispose();
                hv_Row1 = 1040;
                hv_Column1.Dispose();
                hv_Column1 = 905;
                hv_Row2.Dispose();
                hv_Row2 = 3525;
                hv_Column2.Dispose();
                hv_Column2 = 4570;

                ho_Rectangle.Dispose();
                HOperatorSet.GenRectangle1(out ho_Rectangle, hv_Row1, hv_Column1, hv_Row2, hv_Column2);

                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_GrayImage, ho_Rectangle, out ho_ImageReduced);

                ho_Regions.Dispose();
                HOperatorSet.Threshold(ho_ImageReduced, out ho_Regions, 0, 100);

                ho_RegionClosing.Dispose();
                HOperatorSet.ClosingCircle(ho_Regions, out ho_RegionClosing, 10);

                ho_RegionOpening.Dispose();
                HOperatorSet.OpeningCircle(ho_RegionClosing, out ho_RegionOpening, 10);

                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_RegionOpening, out ho_ConnectedRegions);

                ho_SortedRegions.Dispose();
                HOperatorSet.SortRegion(ho_ConnectedRegions, out ho_SortedRegions, "first_point", "true", "row");

                ho_ObjectSelected1.Dispose();
                HOperatorSet.SelectObj(ho_SortedRegions, out ho_ObjectSelected1, 1);
                ho_ObjectSelected2.Dispose();
                HOperatorSet.SelectObj(ho_SortedRegions, out ho_ObjectSelected2, 2);

                //提取光纤的轮廓并拟合成直线
                ho_Skeleton.Dispose();
                HOperatorSet.Skeleton(ho_ObjectSelected1, out ho_Skeleton);
                ho_RegionClipped.Dispose();
                HOperatorSet.ClipRegionRel(ho_Skeleton, out ho_RegionClipped, 100, 0, 0, 0);
                ho_Contours.Dispose();
                HOperatorSet.GenContoursSkeletonXld(ho_RegionClipped, out ho_Contours, 1, "filter");
                hv_RowBegin.Dispose(); hv_ColBegin.Dispose(); hv_RowEnd.Dispose(); hv_ColEnd.Dispose(); hv_Nr.Dispose(); hv_Nc.Dispose(); hv_Dist.Dispose();
                HOperatorSet.FitLineContourXld(ho_Contours, "tukey", -1, 0, 5, 2, out hv_RowBegin, out hv_ColBegin, out hv_RowEnd, out hv_ColEnd, out hv_Nr, out hv_Nc, out hv_Dist);

                ho_RegionBorder.Dispose();
                HOperatorSet.Boundary(ho_ObjectSelected2, out ho_RegionBorder, "outer");
                ho_RegionClipped1.Dispose();
                HOperatorSet.ClipRegionRel(ho_RegionBorder, out ho_RegionClipped1, 0, 100, 100, 100);
                ho_Skeleton2.Dispose();
                HOperatorSet.Skeleton(ho_RegionClipped1, out ho_Skeleton2);
                ho_Contours2.Dispose();
                HOperatorSet.GenContoursSkeletonXld(ho_Skeleton2, out ho_Contours2, 1, "filter");
                hv_RowBegin1.Dispose(); hv_ColBegin1.Dispose(); hv_RowEnd1.Dispose(); hv_ColEnd1.Dispose(); hv_Nr1.Dispose(); hv_Nc1.Dispose(); hv_Dist1.Dispose();
                HOperatorSet.FitLineContourXld(ho_Contours2, "tukey", -1, 0, 5, 2, out hv_RowBegin1, out hv_ColBegin1, out hv_RowEnd1, out hv_ColEnd1, out hv_Nr1, out hv_Nc1, out hv_Dist1);

                hv_Angle.Dispose();
                if (hv_ColBegin1.D > hv_ColEnd1.D)
                {
                    HOperatorSet.AngleLl(hv_RowBegin, hv_ColBegin, hv_RowEnd, hv_ColEnd, hv_RowEnd1, hv_ColEnd1, hv_RowBegin1, hv_ColBegin1, out hv_Angle);
                }
                else
                {
                    HOperatorSet.AngleLl(hv_RowBegin, hv_ColBegin, hv_RowEnd, hv_ColEnd, hv_RowBegin1, hv_ColBegin1, hv_RowEnd1, hv_ColEnd1, out hv_Angle);
                }

                hv_Deg.Dispose();
                HOperatorSet.TupleDeg(hv_Angle, out hv_Deg);

                if (isLeft)
                {
                    angle = 90 - hv_Deg.D;
                }
                else
                {
                    angle = hv_Deg.D - 90;
                }

                hv_Row.Dispose(); hv_Column.Dispose(); hv_IsOverlapping.Dispose();
                HOperatorSet.IntersectionLines(hv_RowBegin, hv_ColBegin, hv_RowEnd, hv_ColEnd, hv_RowBegin1, hv_ColBegin1, hv_RowEnd1, hv_ColEnd1, out hv_Row, out hv_Column, out hv_IsOverlapping);

                ho_RegionLines.Dispose();
                HOperatorSet.GenRegionLine(out ho_RegionLines, hv_RowBegin, hv_ColBegin, hv_Row, hv_Column);
                ho_RegionLines1.Dispose();
                HOperatorSet.GenRegionLine(out ho_RegionLines1, hv_RowBegin1, hv_ColBegin1, hv_RowEnd1, hv_ColEnd1);

                //显示结果
                HOperatorSet.SetColor(hv_WindowHandle, "red");
                HOperatorSet.ClearWindow(hv_WindowHandle);
                HOperatorSet.DispObj(ho_L1, hv_WindowHandle);
                HOperatorSet.DispObj(ho_RegionLines, hv_WindowHandle);
                HOperatorSet.DispObj(ho_RegionLines1, hv_WindowHandle);
                HOperatorSet.DispText(hv_WindowHandle, angle.ToString("f4") + "°", "image", hv_Row - 250, hv_Column + 50, "black", new HTuple(), new HTuple());

                bRet = true;
            }
            catch (Exception ex)
            {
                throw(ex);
            }
            finally
            {
                ho_L1.Dispose();
                ho_GrayImage.Dispose();
                ho_Rectangle.Dispose();
                ho_ImageReduced.Dispose();
                ho_Regions.Dispose();
                ho_RegionClosing.Dispose();
                ho_RegionOpening.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SortedRegions.Dispose();
                ho_ObjectSelected1.Dispose();
                ho_ObjectSelected2.Dispose();
                ho_Skeleton.Dispose();
                ho_RegionClipped.Dispose();
                ho_Contours.Dispose();
                ho_RegionBorder.Dispose();
                ho_RegionClipped1.Dispose();
                ho_Skeleton2.Dispose();
                ho_Contours2.Dispose();
                ho_RegionLines.Dispose();
                ho_RegionLines1.Dispose();

                hv_WindowHandle.Dispose();
                hv_Row1.Dispose();
                hv_Column1.Dispose();
                hv_Row2.Dispose();
                hv_Column2.Dispose();
                hv_RowBegin.Dispose();
                hv_ColBegin.Dispose();
                hv_RowEnd.Dispose();
                hv_ColEnd.Dispose();
                hv_Nr.Dispose();
                hv_Nc.Dispose();
                hv_Dist.Dispose();
                hv_RowBegin1.Dispose();
                hv_ColBegin1.Dispose();
                hv_RowEnd1.Dispose();
                hv_ColEnd1.Dispose();
                hv_Nr1.Dispose();
                hv_Nc1.Dispose();
                hv_Dist1.Dispose();
                hv_Angle.Dispose();
                hv_Deg.Dispose();
                hv_Row.Dispose();
                hv_Column.Dispose();
                hv_IsOverlapping.Dispose();
            }

            return bRet;
        }

        internal static bool GetFiberPoint(HObject ho_L1, HTuple hv_WindowHandle, out double row, out double col)
        {
            row = 0;
            col = 0;
            // Local iconic variables 
            HObject ho_GrayImage, ho_Rectangle;
            HObject ho_ImageReduced, ho_Regions, ho_RegionClosing, ho_RegionOpening;
            HObject ho_ConnectedRegions, ho_SortedRegions, ho_ObjectSelected1;
            HObject ho_Skeleton, ho_RegionClipped, ho_Contours, ho_FiberRegionBorder;
            HObject ho_RegionClipped2, ho_Skeleton1, ho_Contours1;
            bool bRet = false;

            // Local control variables 
            HTuple hv_Row1 = new HTuple();
            HTuple hv_Column1 = new HTuple(), hv_Row2 = new HTuple();
            HTuple hv_Column2 = new HTuple(), hv_RowBegin = new HTuple();
            HTuple hv_ColBegin = new HTuple(), hv_RowEnd = new HTuple();
            HTuple hv_ColEnd = new HTuple(), hv_Nr = new HTuple();
            HTuple hv_Nc = new HTuple(), hv_Dist = new HTuple(), hv_Row = new HTuple();
            HTuple hv_Column = new HTuple(), hv_IsOverlapping = new HTuple();
            // Initialize local and output iconic variables 

            HOperatorSet.GenEmptyObj(out ho_GrayImage);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SortedRegions);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected1);
            HOperatorSet.GenEmptyObj(out ho_Skeleton);
            HOperatorSet.GenEmptyObj(out ho_RegionClipped);
            HOperatorSet.GenEmptyObj(out ho_Contours);
            HOperatorSet.GenEmptyObj(out ho_FiberRegionBorder);
            HOperatorSet.GenEmptyObj(out ho_RegionClipped2);
            HOperatorSet.GenEmptyObj(out ho_Skeleton1);
            HOperatorSet.GenEmptyObj(out ho_Contours1);

            try
            {
                HOperatorSet.ClearWindow(hv_WindowHandle);
                HOperatorSet.DispObj(ho_L1, hv_WindowHandle);
                HOperatorSet.SetColor(hv_WindowHandle, "red");

                ho_GrayImage.Dispose();
                HOperatorSet.Rgb1ToGray(ho_L1, out ho_GrayImage);

                hv_Row1.Dispose();
                hv_Row1 = 1040;
                hv_Column1.Dispose();
                hv_Column1 = 905;
                hv_Row2.Dispose();
                hv_Row2 = 3525;
                hv_Column2.Dispose();
                hv_Column2 = 4570;

                ho_Rectangle.Dispose();
                HOperatorSet.GenRectangle1(out ho_Rectangle, hv_Row1, hv_Column1, hv_Row2, hv_Column2);

                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_GrayImage, ho_Rectangle, out ho_ImageReduced);

                ho_Regions.Dispose();
                HOperatorSet.Threshold(ho_ImageReduced, out ho_Regions, 0, 100);

                ho_RegionClosing.Dispose();
                HOperatorSet.ClosingCircle(ho_Regions, out ho_RegionClosing, 10);

                ho_RegionOpening.Dispose();
                HOperatorSet.OpeningCircle(ho_RegionClosing, out ho_RegionOpening, 10);

                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_RegionOpening, out ho_ConnectedRegions);

                ho_SortedRegions.Dispose();
                HOperatorSet.SortRegion(ho_ConnectedRegions, out ho_SortedRegions, "first_point", "true", "row");

                ho_ObjectSelected1.Dispose();
                HOperatorSet.SelectObj(ho_SortedRegions, out ho_ObjectSelected1, 1);

                //提取光纤的轮廓并拟合成直线
                ho_Skeleton.Dispose();
                HOperatorSet.Skeleton(ho_ObjectSelected1, out ho_Skeleton);
                ho_RegionClipped.Dispose();
                HOperatorSet.ClipRegionRel(ho_Skeleton, out ho_RegionClipped, 100, 0, 0, 0);
                ho_Contours.Dispose();
                HOperatorSet.GenContoursSkeletonXld(ho_RegionClipped, out ho_Contours, 1, "filter");
                hv_RowBegin.Dispose(); hv_ColBegin.Dispose(); hv_RowEnd.Dispose(); hv_ColEnd.Dispose(); hv_Nr.Dispose(); hv_Nc.Dispose(); hv_Dist.Dispose();
                HOperatorSet.FitLineContourXld(ho_Contours, "tukey", -1, 0, 5, 2, out hv_RowBegin,
                    out hv_ColBegin, out hv_RowEnd, out hv_ColEnd, out hv_Nr, out hv_Nc, out hv_Dist);
                //提取光纤的外轮廓
                ho_FiberRegionBorder.Dispose();
                HOperatorSet.Boundary(ho_ObjectSelected1, out ho_FiberRegionBorder, "outer");
                ho_RegionClipped2.Dispose();
                HOperatorSet.ClipRegionRel(ho_FiberRegionBorder, out ho_RegionClipped2, 100, 0, 0, 0);
                ho_Skeleton1.Dispose();
                HOperatorSet.Skeleton(ho_RegionClipped2, out ho_Skeleton1);
                ho_Contours1.Dispose();
                HOperatorSet.GenContoursSkeletonXld(ho_Skeleton1, out ho_Contours1, 1, "filter");
                //求交点
                hv_Row.Dispose(); hv_Column.Dispose(); hv_IsOverlapping.Dispose();
                HOperatorSet.IntersectionLineContourXld(ho_Contours1, hv_RowBegin, hv_ColBegin,
                    hv_RowEnd, hv_ColEnd, out hv_Row, out hv_Column, out hv_IsOverlapping);

                if (hv_Row.Length < 1)
                {
                    throw new Exception("GetFiberPoint()出错。没有找到交点。");
                }
                else
                {
                    row = hv_Row.D;
                    col = hv_Column.D;
                    HOperatorSet.DispCross(hv_WindowHandle, hv_Row, hv_Column, 200, 0);

                    bRet = true;
                }
            }
            catch (Exception ex)
            {
                throw(ex);
            }
            finally
            {
                ho_L1.Dispose();
                ho_GrayImage.Dispose();
                ho_Rectangle.Dispose();
                ho_ImageReduced.Dispose();
                ho_Regions.Dispose();
                ho_RegionClosing.Dispose();
                ho_RegionOpening.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SortedRegions.Dispose();
                ho_ObjectSelected1.Dispose();
                ho_Skeleton.Dispose();
                ho_RegionClipped.Dispose();
                ho_Contours.Dispose();
                ho_FiberRegionBorder.Dispose();
                ho_RegionClipped2.Dispose();
                ho_Skeleton1.Dispose();
                ho_Contours1.Dispose();

                hv_WindowHandle.Dispose();
                hv_Row1.Dispose();
                hv_Column1.Dispose();
                hv_Row2.Dispose();
                hv_Column2.Dispose();
                hv_RowBegin.Dispose();
                hv_ColBegin.Dispose();
                hv_RowEnd.Dispose();
                hv_ColEnd.Dispose();
                hv_Nr.Dispose();
                hv_Nc.Dispose();
                hv_Dist.Dispose();
                hv_Row.Dispose();
                hv_Column.Dispose();
                hv_IsOverlapping.Dispose();
            }

            return bRet;
        }

        public static bool GetFAAndPlatDistance(HObject ho_L1, HTuple hv_WindowHandle, out double distancePix)
        {
            distancePix = 0;
            bool bRet = false;  

            // Local iconic variables 
            HObject ho_GrayImage, ho_Rectangle;
            HObject ho_ImageReduced, ho_Regions, ho_RegionClosing, ho_RegionOpening;
            HObject ho_ConnectedRegions, ho_SortedRegions, ho_FA, ho_RegionBorder;
            HObject ho_RegionClipped, ho_Skeleton, ho_Contours, ho_SelectedContours;
            HObject ho_ContoursSplit, ho_SortedContours, ho_Plat, ho_RegionBorder1;
            HObject ho_RegionClipped1, ho_Skeleton1, ho_Contours1, ho_RegionLines1;

            // Local control variables 
            HTuple hv_row1 = new HTuple(), hv_col1 = new HTuple();
            HTuple hv_row2 = new HTuple(), hv_col2 = new HTuple();
            HTuple hv_RowBegin = new HTuple(), hv_ColBegin = new HTuple();
            HTuple hv_RowEnd = new HTuple(), hv_ColEnd = new HTuple();
            HTuple hv_Nr = new HTuple(), hv_Nc = new HTuple(), hv_Dist = new HTuple();
            HTuple hv_RowL = new HTuple(), hv_ColumnL = new HTuple();
            HTuple hv_IsOverlapping = new HTuple(), hv_RowR = new HTuple();
            HTuple hv_ColumnR = new HTuple();
            HTuple hv_RowBegin1 = new HTuple(), hv_ColBegin1 = new HTuple();
            HTuple hv_RowEnd1 = new HTuple(), hv_ColEnd1 = new HTuple();
            HTuple hv_Nr1 = new HTuple(), hv_Nc1 = new HTuple(), hv_Dist1 = new HTuple();
            HTuple hv_Distance1 = new HTuple(), hv_Distance2 = new HTuple();
            HTuple hv_Min2 = new HTuple();

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_GrayImage);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SortedRegions);
            HOperatorSet.GenEmptyObj(out ho_FA);
            HOperatorSet.GenEmptyObj(out ho_RegionBorder);
            HOperatorSet.GenEmptyObj(out ho_RegionClipped);
            HOperatorSet.GenEmptyObj(out ho_Skeleton);
            HOperatorSet.GenEmptyObj(out ho_Contours);
            HOperatorSet.GenEmptyObj(out ho_SelectedContours);
            HOperatorSet.GenEmptyObj(out ho_ContoursSplit);
            HOperatorSet.GenEmptyObj(out ho_SortedContours);
            HOperatorSet.GenEmptyObj(out ho_Plat);
            HOperatorSet.GenEmptyObj(out ho_RegionBorder1);
            HOperatorSet.GenEmptyObj(out ho_RegionClipped1);
            HOperatorSet.GenEmptyObj(out ho_Skeleton1);
            HOperatorSet.GenEmptyObj(out ho_Contours1);
            HOperatorSet.GenEmptyObj(out ho_RegionLines1);

            ho_GrayImage.Dispose();
            HOperatorSet.Rgb1ToGray(ho_L1, out ho_GrayImage);

            hv_row1.Dispose();
            hv_row1 = 1235.5;
            hv_col1.Dispose();
            hv_col1 = 1051.5;
            hv_row2.Dispose();
            hv_row2 = 3403.5;
            hv_col2.Dispose();
            hv_col2 = 4635.5;

            try
            {
                ho_Rectangle.Dispose();
                HOperatorSet.GenRectangle1(out ho_Rectangle, hv_row1, hv_col1, hv_row2, hv_col2);
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_GrayImage, ho_Rectangle, out ho_ImageReduced);

                ho_Regions.Dispose();
                HOperatorSet.Threshold(ho_ImageReduced, out ho_Regions, 4, 240);
                ho_RegionClosing.Dispose();
                HOperatorSet.ClosingCircle(ho_Regions, out ho_RegionClosing, 20);
                ho_RegionOpening.Dispose();
                HOperatorSet.OpeningCircle(ho_RegionClosing, out ho_RegionOpening, 20);

                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_RegionOpening, out ho_ConnectedRegions);
                ho_SortedRegions.Dispose();
                HOperatorSet.SortRegion(ho_ConnectedRegions, out ho_SortedRegions, "first_point", "true", "row");

                //识别FA的边2
                ho_FA.Dispose();
                HOperatorSet.SelectObj(ho_SortedRegions, out ho_FA, 1);
                ho_RegionBorder.Dispose();
                HOperatorSet.Boundary(ho_FA, out ho_RegionBorder, "inner");
                ho_RegionClipped.Dispose();
                HOperatorSet.ClipRegionRel(ho_RegionBorder, out ho_RegionClipped, 20, 0, 0, 0);
                ho_Skeleton.Dispose();
                HOperatorSet.Skeleton(ho_RegionClipped, out ho_Skeleton);
                ho_Contours.Dispose();
                HOperatorSet.GenContoursSkeletonXld(ho_Skeleton, out ho_Contours, 1, "filter");
                ho_ContoursSplit.Dispose();
                HOperatorSet.SegmentContoursXld(ho_Contours, out ho_ContoursSplit, "lines_circles", 9, 40, 20);
                ho_SelectedContours.Dispose();
                HOperatorSet.SelectContoursXld(ho_ContoursSplit, out ho_SelectedContours, "contour_length", 300, 2000, -0.5, 0.5);
                ho_SortedContours.Dispose();
                HOperatorSet.SortContoursXld(ho_SelectedContours, out ho_SortedContours, "upper_left", "true", "column");
                hv_RowBegin.Dispose(); hv_ColBegin.Dispose(); hv_RowEnd.Dispose(); hv_ColEnd.Dispose(); hv_Nr.Dispose(); hv_Nc.Dispose(); hv_Dist.Dispose();
                HOperatorSet.FitLineContourXld(ho_SelectedContours, "tukey", -1, 0, 5, 2, out hv_RowBegin,
                    out hv_ColBegin, out hv_RowEnd, out hv_ColEnd, out hv_Nr, out hv_Nc, out hv_Dist);
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_RowL.Dispose(); hv_ColumnL.Dispose(); hv_IsOverlapping.Dispose();
                    HOperatorSet.IntersectionLines(hv_RowBegin.TupleSelect(0), hv_ColBegin.TupleSelect(
                        0), hv_RowEnd.TupleSelect(0), hv_ColEnd.TupleSelect(0), hv_RowBegin.TupleSelect(
                        1), hv_ColBegin.TupleSelect(1), hv_RowEnd.TupleSelect(1), hv_ColEnd.TupleSelect(
                        1), out hv_RowL, out hv_ColumnL, out hv_IsOverlapping);
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_RowR.Dispose(); hv_ColumnR.Dispose(); hv_IsOverlapping.Dispose();
                    HOperatorSet.IntersectionLines(hv_RowBegin.TupleSelect(2), hv_ColBegin.TupleSelect(
                        2), hv_RowEnd.TupleSelect(2), hv_ColEnd.TupleSelect(2), hv_RowBegin.TupleSelect(
                        1), hv_ColBegin.TupleSelect(1), hv_RowEnd.TupleSelect(1), hv_ColEnd.TupleSelect(
                        1), out hv_RowR, out hv_ColumnR, out hv_IsOverlapping);
                }

                HOperatorSet.DispObj(ho_L1, hv_WindowHandle);
                HOperatorSet.DispCross(hv_WindowHandle, hv_RowL, hv_ColumnL, 50, 0);
                HOperatorSet.DispCross(hv_WindowHandle, hv_RowR, hv_ColumnR, 50, 0);

                //识别plat的边
                ho_Plat.Dispose();
                HOperatorSet.SelectObj(ho_SortedRegions, out ho_Plat, 2);
                ho_RegionBorder1.Dispose();
                HOperatorSet.Boundary(ho_Plat, out ho_RegionBorder1, "inner");
                ho_RegionClipped1.Dispose();
                HOperatorSet.ClipRegionRel(ho_RegionBorder1, out ho_RegionClipped1, 0, 100, 100,
                    100);
                ho_Skeleton1.Dispose();
                HOperatorSet.Skeleton(ho_RegionClipped1, out ho_Skeleton1);
                ho_Contours1.Dispose();
                HOperatorSet.GenContoursSkeletonXld(ho_Skeleton1, out ho_Contours1, 1, "filter");
                hv_RowBegin1.Dispose(); hv_ColBegin1.Dispose(); hv_RowEnd1.Dispose(); hv_ColEnd1.Dispose(); hv_Nr1.Dispose(); hv_Nc1.Dispose(); hv_Dist1.Dispose();
                HOperatorSet.FitLineContourXld(ho_Contours1, "tukey", -1, 0, 5, 2, out hv_RowBegin1,
                    out hv_ColBegin1, out hv_RowEnd1, out hv_ColEnd1, out hv_Nr1, out hv_Nc1,
                    out hv_Dist1);
                ho_RegionLines1.Dispose();
                HOperatorSet.GenRegionLine(out ho_RegionLines1, hv_RowBegin1, hv_ColBegin1, hv_RowEnd1,hv_ColEnd1);
                HOperatorSet.DispObj(ho_RegionLines1, hv_WindowHandle);

                //计算FA到plat距离
                hv_Distance1.Dispose();
                HOperatorSet.DistancePl(hv_RowL, hv_ColumnL, hv_RowBegin1, hv_ColBegin1, hv_RowEnd1,hv_ColEnd1, out hv_Distance1);
                hv_Distance2.Dispose();
                HOperatorSet.DistancePl(hv_RowR, hv_ColumnR, hv_RowBegin1, hv_ColBegin1, hv_RowEnd1,hv_ColEnd1, out hv_Distance2);
                hv_Min2.Dispose();
                HOperatorSet.TupleMin2(hv_Distance1, hv_Distance2, out hv_Min2);

                distancePix = hv_Min2.D;
                bRet = true;
            }
            catch (Exception ex)
            {
                throw(ex);
            }
            finally
            {
                ho_GrayImage.Dispose();
                ho_Rectangle.Dispose();
                ho_ImageReduced.Dispose();
                ho_Regions.Dispose();
                ho_RegionClosing.Dispose();
                ho_RegionOpening.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SortedRegions.Dispose();
                ho_FA.Dispose();
                ho_RegionBorder.Dispose();
                ho_RegionClipped.Dispose();
                ho_Skeleton.Dispose();
                ho_Contours.Dispose();
                ho_SelectedContours.Dispose();
                ho_ContoursSplit.Dispose();
                ho_SortedContours.Dispose();
                ho_Plat.Dispose();
                ho_RegionBorder1.Dispose();
                ho_RegionClipped1.Dispose();
                ho_Skeleton1.Dispose();
                ho_Contours1.Dispose();
                ho_RegionLines1.Dispose();

                hv_row1.Dispose();
                hv_col1.Dispose();
                hv_row2.Dispose();
                hv_col2.Dispose();
                hv_RowBegin.Dispose();
                hv_ColBegin.Dispose();
                hv_RowEnd.Dispose();
                hv_ColEnd.Dispose();
                hv_Nr.Dispose();
                hv_Nc.Dispose();
                hv_Dist.Dispose();
                hv_RowL.Dispose();
                hv_ColumnL.Dispose();
                hv_IsOverlapping.Dispose();
                hv_RowR.Dispose();
                hv_ColumnR.Dispose();
                hv_WindowHandle.Dispose();
                hv_RowBegin1.Dispose();
                hv_ColBegin1.Dispose();
                hv_RowEnd1.Dispose();
                hv_ColEnd1.Dispose();
                hv_Nr1.Dispose();
                hv_Nc1.Dispose();
                hv_Dist1.Dispose();
                hv_Distance1.Dispose();
                hv_Distance2.Dispose();
                hv_Min2.Dispose();
            }

            return bRet;
        }

        public static bool GetFiberAndPlatDistance(HObject ho_L1, HTuple hv_WindowHandle, out double distancePix)
        {
            distancePix = 0;
            bool bRet = false;

            // Local iconic variables 
            HObject ho_GrayImage, ho_Rectangle;
            HObject ho_ImageReduced, ho_Regions, ho_RegionClosing, ho_RegionOpening;
            HObject ho_ConnectedRegions, ho_SortedRegions, ho_ObjectSelected1;
            HObject ho_ObjectSelected2, ho_Skeleton, ho_RegionClipped;
            HObject ho_Contours, ho_FiberRegionBorder, ho_RegionClipped2;
            HObject ho_Skeleton1, ho_Contours1, ho_RegionBorder, ho_RegionClipped1;
            HObject ho_Skeleton2, ho_Contours2;

            // Local control variables  hv_WindowHandle = new HTuple(),
            HTuple hv_Row1 = new HTuple();
            HTuple hv_Column1 = new HTuple(), hv_Row2 = new HTuple();
            HTuple hv_Column2 = new HTuple(), hv_RowBegin = new HTuple();
            HTuple hv_ColBegin = new HTuple(), hv_RowEnd = new HTuple();
            HTuple hv_ColEnd = new HTuple(), hv_Nr = new HTuple();
            HTuple hv_Nc = new HTuple(), hv_Dist = new HTuple(), hv_Row = new HTuple();
            HTuple hv_Column = new HTuple(), hv_IsOverlapping = new HTuple();
            HTuple hv_RowBegin1 = new HTuple(), hv_ColBegin1 = new HTuple();
            HTuple hv_RowEnd1 = new HTuple(), hv_ColEnd1 = new HTuple();
            HTuple hv_Nr1 = new HTuple(), hv_Nc1 = new HTuple(), hv_Dist1 = new HTuple();
            HTuple hv_Distance = new HTuple();

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_GrayImage);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SortedRegions);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected1);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected2);
            HOperatorSet.GenEmptyObj(out ho_Skeleton);
            HOperatorSet.GenEmptyObj(out ho_RegionClipped);
            HOperatorSet.GenEmptyObj(out ho_Contours);
            HOperatorSet.GenEmptyObj(out ho_FiberRegionBorder);
            HOperatorSet.GenEmptyObj(out ho_RegionClipped2);
            HOperatorSet.GenEmptyObj(out ho_Skeleton1);
            HOperatorSet.GenEmptyObj(out ho_Contours1);
            HOperatorSet.GenEmptyObj(out ho_RegionBorder);
            HOperatorSet.GenEmptyObj(out ho_RegionClipped1);
            HOperatorSet.GenEmptyObj(out ho_Skeleton2);
            HOperatorSet.GenEmptyObj(out ho_Contours2);

            try
            {
                ho_GrayImage.Dispose();
                HOperatorSet.Rgb1ToGray(ho_L1, out ho_GrayImage);

                hv_Row1.Dispose();
                hv_Row1 = 640;
                hv_Column1.Dispose();
                hv_Column1 = 905;
                hv_Row2.Dispose();
                hv_Row2 = 3225;
                hv_Column2.Dispose();
                hv_Column2 = 4570;

                ho_Rectangle.Dispose();
                HOperatorSet.GenRectangle1(out ho_Rectangle, hv_Row1, hv_Column1, hv_Row2, hv_Column2);

                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_GrayImage, ho_Rectangle, out ho_ImageReduced);

                ho_Regions.Dispose();
                HOperatorSet.Threshold(ho_ImageReduced, out ho_Regions, 0, 100);

                ho_RegionClosing.Dispose();
                HOperatorSet.ClosingCircle(ho_Regions, out ho_RegionClosing, 10);

                ho_RegionOpening.Dispose();
                HOperatorSet.OpeningCircle(ho_RegionClosing, out ho_RegionOpening, 10);

                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_RegionOpening, out ho_ConnectedRegions);

                ho_SortedRegions.Dispose();
                HOperatorSet.SortRegion(ho_ConnectedRegions, out ho_SortedRegions, "first_point", "true", "row");

                ho_ObjectSelected1.Dispose();
                HOperatorSet.SelectObj(ho_SortedRegions, out ho_ObjectSelected1, 1);
                ho_ObjectSelected2.Dispose();
                HOperatorSet.SelectObj(ho_SortedRegions, out ho_ObjectSelected2, 2);

                //提取光纤的轮廓并拟合成直线
                ho_Skeleton.Dispose();
                HOperatorSet.Skeleton(ho_ObjectSelected1, out ho_Skeleton);
                ho_RegionClipped.Dispose();
                HOperatorSet.ClipRegionRel(ho_Skeleton, out ho_RegionClipped, 100, 0, 0, 0);
                ho_Contours.Dispose();
                HOperatorSet.GenContoursSkeletonXld(ho_RegionClipped, out ho_Contours, 1, "filter");
                hv_RowBegin.Dispose(); hv_ColBegin.Dispose(); hv_RowEnd.Dispose(); hv_ColEnd.Dispose(); hv_Nr.Dispose(); hv_Nc.Dispose(); hv_Dist.Dispose();
                HOperatorSet.FitLineContourXld(ho_Contours, "tukey", -1, 0, 5, 2, out hv_RowBegin,
                    out hv_ColBegin, out hv_RowEnd, out hv_ColEnd, out hv_Nr, out hv_Nc, out hv_Dist);
                //提取光纤的外轮廓
                ho_FiberRegionBorder.Dispose();
                HOperatorSet.Boundary(ho_ObjectSelected1, out ho_FiberRegionBorder, "outer");
                ho_RegionClipped2.Dispose();
                HOperatorSet.ClipRegionRel(ho_FiberRegionBorder, out ho_RegionClipped2, 100, 0, 0, 0);
                ho_Skeleton1.Dispose();
                HOperatorSet.Skeleton(ho_RegionClipped2, out ho_Skeleton1);
                ho_Contours1.Dispose();
                HOperatorSet.GenContoursSkeletonXld(ho_Skeleton1, out ho_Contours1, 1, "filter");
                //求交点
                hv_Row.Dispose(); hv_Column.Dispose(); hv_IsOverlapping.Dispose();
                HOperatorSet.IntersectionLineContourXld(ho_Contours1, hv_RowBegin, hv_ColBegin, hv_RowEnd, hv_ColEnd, out hv_Row, out hv_Column, out hv_IsOverlapping);

                //识别标定板的上边缘
                ho_RegionBorder.Dispose();
                HOperatorSet.Boundary(ho_ObjectSelected2, out ho_RegionBorder, "outer");
                ho_RegionClipped1.Dispose();
                HOperatorSet.ClipRegionRel(ho_RegionBorder, out ho_RegionClipped1, 0, 100, 100, 100);
                ho_Skeleton2.Dispose();
                HOperatorSet.Skeleton(ho_RegionClipped1, out ho_Skeleton2);
                ho_Contours2.Dispose();
                HOperatorSet.GenContoursSkeletonXld(ho_Skeleton2, out ho_Contours2, 1, "filter");
                hv_RowBegin1.Dispose(); hv_ColBegin1.Dispose(); hv_RowEnd1.Dispose(); hv_ColEnd1.Dispose(); hv_Nr1.Dispose(); hv_Nc1.Dispose(); hv_Dist1.Dispose();
                HOperatorSet.FitLineContourXld(ho_Contours2, "tukey", -1, 0, 5, 2, out hv_RowBegin1,
                    out hv_ColBegin1, out hv_RowEnd1, out hv_ColEnd1, out hv_Nr1, out hv_Nc1, out hv_Dist1);

                hv_Distance.Dispose();
                HOperatorSet.DistancePl(hv_Row, hv_Column, hv_RowBegin1, hv_ColBegin1, hv_RowEnd1, hv_ColEnd1, out hv_Distance);
                distancePix = hv_Distance.D;

                HOperatorSet.SetColor(hv_WindowHandle, "red");
                HOperatorSet.DispCross(hv_WindowHandle, hv_Row, hv_Column, 200, 0);
                HOperatorSet.DispLine(hv_WindowHandle, hv_RowBegin1, hv_ColBegin1, hv_RowEnd1, hv_ColEnd1);

                bRet = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ho_L1.Dispose();
                ho_GrayImage.Dispose();
                ho_Rectangle.Dispose();
                ho_ImageReduced.Dispose();
                ho_Regions.Dispose();
                ho_RegionClosing.Dispose();
                ho_RegionOpening.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SortedRegions.Dispose();
                ho_ObjectSelected1.Dispose();
                ho_ObjectSelected2.Dispose();
                ho_Skeleton.Dispose();
                ho_RegionClipped.Dispose();
                ho_Contours.Dispose();
                ho_FiberRegionBorder.Dispose();
                ho_RegionClipped2.Dispose();
                ho_Skeleton1.Dispose();
                ho_Contours1.Dispose();
                ho_RegionBorder.Dispose();
                ho_RegionClipped1.Dispose();
                ho_Skeleton2.Dispose();
                ho_Contours2.Dispose();

                hv_WindowHandle.Dispose();
                hv_Row1.Dispose();
                hv_Column1.Dispose();
                hv_Row2.Dispose();
                hv_Column2.Dispose();
                hv_RowBegin.Dispose();
                hv_ColBegin.Dispose();
                hv_RowEnd.Dispose();
                hv_ColEnd.Dispose();
                hv_Nr.Dispose();
                hv_Nc.Dispose();
                hv_Dist.Dispose();
                hv_Row.Dispose();
                hv_Column.Dispose();
                hv_IsOverlapping.Dispose();
                hv_RowBegin1.Dispose();
                hv_ColBegin1.Dispose();
                hv_RowEnd1.Dispose();
                hv_ColEnd1.Dispose();
                hv_Nr1.Dispose();
                hv_Nc1.Dispose();
                hv_Dist1.Dispose();
                hv_Distance.Dispose();
            }

            return bRet;
        }

        internal static void GetSYBetweenFaAndPlat(bool isLeft, HObject image, HWindow halconWindow, out double angle)
        {
            angle = 0;

            // Local iconic variables 
            HObject ho_Fa1, ho_GrayImage, ho_Rectangle;
            HObject ho_ImageReduced, ho_Regions, ho_RegionClosing, ho_RegionOpening;
            HObject ho_ConnectedRegions, ho_SortedRegions, ho_FA, ho_RegionBorder;
            HObject ho_RegionClipped, ho_Skeleton, ho_Contours, ho_SelectedContours;
            HObject ho_RegionLines, ho_Plat, ho_RegionBorder1, ho_RegionClipped1;
            HObject ho_Skeleton1, ho_Contours1, ho_RegionLines1;

            // Local control variables 
            HTuple hv_row1 = new HTuple(), hv_col1 = new HTuple();
            HTuple hv_row2 = new HTuple(), hv_col2 = new HTuple();
            HTuple hv_RowBegin = new HTuple(), hv_ColBegin = new HTuple();
            HTuple hv_RowEnd = new HTuple(), hv_ColEnd = new HTuple();
            HTuple hv_Nr = new HTuple(), hv_Nc = new HTuple(), hv_Dist = new HTuple();
            HTuple hv_RowBegin1 = new HTuple(), hv_ColBegin1 = new HTuple();
            HTuple hv_RowEnd1 = new HTuple(), hv_ColEnd1 = new HTuple();
            HTuple hv_Nr1 = new HTuple(), hv_Nc1 = new HTuple(), hv_Dist1 = new HTuple();
            HTuple hv_faRow1 = new HTuple(), hv_faCol1 = new HTuple();
            HTuple hv_faRow2 = new HTuple(), hv_faCol2 = new HTuple();
            HTuple hv_faRow3 = new HTuple(), hv_faCol3 = new HTuple();
            HTuple hv_faRow4 = new HTuple(), hv_faCol4 = new HTuple();
            HTuple hv_Angle = new HTuple(), hv_Deg = new HTuple();

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Fa1);
            HOperatorSet.GenEmptyObj(out ho_GrayImage);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SortedRegions);
            HOperatorSet.GenEmptyObj(out ho_FA);
            HOperatorSet.GenEmptyObj(out ho_RegionBorder);
            HOperatorSet.GenEmptyObj(out ho_RegionClipped);
            HOperatorSet.GenEmptyObj(out ho_Skeleton);
            HOperatorSet.GenEmptyObj(out ho_Contours);
            HOperatorSet.GenEmptyObj(out ho_SelectedContours);
            HOperatorSet.GenEmptyObj(out ho_RegionLines);
            HOperatorSet.GenEmptyObj(out ho_Plat);
            HOperatorSet.GenEmptyObj(out ho_RegionBorder1);
            HOperatorSet.GenEmptyObj(out ho_RegionClipped1);
            HOperatorSet.GenEmptyObj(out ho_Skeleton1);
            HOperatorSet.GenEmptyObj(out ho_Contours1);
            HOperatorSet.GenEmptyObj(out ho_RegionLines1);

            try
            {
                ho_GrayImage.Dispose();
                HOperatorSet.Rgb1ToGray(image, out ho_GrayImage);

                hv_row1.Dispose();
                hv_row1 = 1035.5;
                hv_col1.Dispose();
                hv_col1 = 1051.5;
                hv_row2.Dispose();
                hv_row2 = 3403.5;
                hv_col2.Dispose();
                hv_col2 = 4635.5;

                ho_Rectangle.Dispose();
                HOperatorSet.GenRectangle1(out ho_Rectangle, hv_row1, hv_col1, hv_row2, hv_col2);
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_GrayImage, ho_Rectangle, out ho_ImageReduced);

                ho_Regions.Dispose();
                HOperatorSet.Threshold(ho_ImageReduced, out ho_Regions, 4, 240);
                ho_RegionClosing.Dispose();
                HOperatorSet.ClosingCircle(ho_Regions, out ho_RegionClosing, 20);
                ho_RegionOpening.Dispose();
                HOperatorSet.OpeningCircle(ho_RegionClosing, out ho_RegionOpening, 20);

                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_RegionOpening, out ho_ConnectedRegions);
                ho_SortedRegions.Dispose();
                HOperatorSet.SortRegion(ho_ConnectedRegions, out ho_SortedRegions, "first_point", "true", "row");
                //识别FA的边1
                ho_FA.Dispose();
                HOperatorSet.SelectObj(ho_SortedRegions, out ho_FA, 1);
                ho_RegionBorder.Dispose();
                HOperatorSet.Boundary(ho_FA, out ho_RegionBorder, "outer");
                ho_RegionClipped.Dispose();
                if(isLeft)
                    HOperatorSet.ClipRegionRel(ho_RegionBorder, out ho_RegionClipped, 20, 0, 130,70);
                else
                    HOperatorSet.ClipRegionRel(ho_RegionBorder, out ho_RegionClipped, 20, 0, 70, 130);
                ho_Skeleton.Dispose();
                HOperatorSet.Skeleton(ho_RegionClipped, out ho_Skeleton);
                ho_Contours.Dispose();
                HOperatorSet.GenContoursSkeletonXld(ho_Skeleton, out ho_Contours, 1, "filter");
                ho_SelectedContours.Dispose();
                HOperatorSet.SelectContoursXld(ho_Contours, out ho_SelectedContours, "contour_length",
                    1000, 2000, -0.5, 0.5);
                hv_RowBegin.Dispose(); hv_ColBegin.Dispose(); hv_RowEnd.Dispose(); hv_ColEnd.Dispose(); hv_Nr.Dispose(); hv_Nc.Dispose(); hv_Dist.Dispose();
                HOperatorSet.FitLineContourXld(ho_SelectedContours, "tukey", -1, 0, 5, 2, out hv_RowBegin,
                    out hv_ColBegin, out hv_RowEnd, out hv_ColEnd, out hv_Nr, out hv_Nc, out hv_Dist);
                ho_RegionLines.Dispose();
                HOperatorSet.GenRegionLine(out ho_RegionLines, hv_RowBegin, hv_ColBegin, hv_RowEnd,
                    hv_ColEnd);
                ho_RegionLines.Dispose();
                HOperatorSet.GenRegionLine(out ho_RegionLines, hv_RowBegin, hv_ColBegin, hv_RowEnd,
                    hv_ColEnd);

                //识别plat的边
                ho_Plat.Dispose();
                HOperatorSet.SelectObj(ho_SortedRegions, out ho_Plat, 2);
                ho_RegionBorder1.Dispose();
                HOperatorSet.Boundary(ho_Plat, out ho_RegionBorder1, "inner");
                ho_RegionClipped1.Dispose();
                HOperatorSet.ClipRegionRel(ho_RegionBorder1, out ho_RegionClipped1, 0, 100, 100,100);
                ho_Skeleton1.Dispose();
                HOperatorSet.Skeleton(ho_RegionClipped1, out ho_Skeleton1);
                ho_Contours1.Dispose();
                HOperatorSet.GenContoursSkeletonXld(ho_Skeleton1, out ho_Contours1, 1, "filter");
                hv_RowBegin1.Dispose(); hv_ColBegin1.Dispose(); hv_RowEnd1.Dispose(); hv_ColEnd1.Dispose(); hv_Nr1.Dispose(); hv_Nc1.Dispose(); hv_Dist1.Dispose();
                HOperatorSet.FitLineContourXld(ho_Contours1, "tukey", -1, 0, 5, 2, out hv_RowBegin1,
                    out hv_ColBegin1, out hv_RowEnd1, out hv_ColEnd1, out hv_Nr1, out hv_Nc1,out hv_Dist1);
                ho_RegionLines1.Dispose();
                HOperatorSet.GenRegionLine(out ho_RegionLines1, hv_RowBegin1, hv_ColBegin1, hv_RowEnd1,hv_ColEnd1);


                //计算FA和plat角度
                hv_faRow1.Dispose();
                hv_faRow1 = new HTuple(hv_RowBegin);
                hv_faCol1.Dispose();
                hv_faCol1 = new HTuple(hv_ColBegin);
                hv_faRow2.Dispose();
                hv_faRow2 = new HTuple(hv_RowEnd);
                hv_faCol2.Dispose();
                hv_faCol2 = new HTuple(hv_ColEnd);
                if ((int)(new HTuple(hv_ColBegin.TupleGreater(hv_ColEnd))) != 0)
                {
                    hv_faRow1.Dispose();
                    hv_faRow1 = new HTuple(hv_RowEnd);
                    hv_faCol1.Dispose();
                    hv_faCol1 = new HTuple(hv_ColEnd);
                    hv_faRow2.Dispose();
                    hv_faRow2 = new HTuple(hv_RowBegin);
                    hv_faCol2.Dispose();
                    hv_faCol2 = new HTuple(hv_ColBegin);
                }

                hv_faRow3.Dispose();
                hv_faRow3 = new HTuple(hv_RowBegin1);
                hv_faCol3.Dispose();
                hv_faCol3 = new HTuple(hv_ColBegin1);
                hv_faRow4.Dispose();
                hv_faRow4 = new HTuple(hv_RowEnd1);
                hv_faCol4.Dispose();
                hv_faCol4 = new HTuple(hv_ColEnd1);
                if ((int)(new HTuple(hv_ColBegin1.TupleGreater(hv_ColEnd1))) != 0)
                {
                    hv_faRow3.Dispose();
                    hv_faRow3 = new HTuple(hv_RowEnd1);
                    hv_faCol3.Dispose();
                    hv_faCol3 = new HTuple(hv_ColEnd1);
                    hv_faRow4.Dispose();
                    hv_faRow4 = new HTuple(hv_RowBegin1);
                    hv_faCol4.Dispose();
                    hv_faCol4 = new HTuple(hv_ColBegin1);
                }

                hv_Angle.Dispose();
                HOperatorSet.AngleLl(hv_faRow3, hv_faCol3, hv_faRow4, hv_faCol4, hv_faRow1, hv_faCol1,hv_faRow2, hv_faCol2, out hv_Angle);
                hv_Deg.Dispose();
                HOperatorSet.TupleDeg(hv_Angle, out hv_Deg);
                if (isLeft)
                    angle = hv_Deg.D;
                else
                    angle = -hv_Deg.D;

                HOperatorSet.ClearWindow(halconWindow);
                HOperatorSet.SetColor(halconWindow,"red");
           
                HOperatorSet.DispImage(image, halconWindow);
                HOperatorSet.DispObj(ho_RegionLines1, halconWindow);
                HOperatorSet.DispObj(ho_RegionLines, halconWindow);
                HOperatorSet.DispText(halconWindow, angle.ToString("f4") + "°", "image", hv_faRow1 + 100,
                            hv_faCol1, "red", new HTuple(), new HTuple());

            }
            catch (Exception ex)
            {
                throw(ex);
            }
            finally
            {
                ho_GrayImage.Dispose();
                ho_Rectangle.Dispose();
                ho_ImageReduced.Dispose();
                ho_Regions.Dispose();
                ho_RegionClosing.Dispose();
                ho_RegionOpening.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SortedRegions.Dispose();
                ho_FA.Dispose();
                ho_RegionBorder.Dispose();
                ho_RegionClipped.Dispose();
                ho_Skeleton.Dispose();
                ho_Contours.Dispose();
                ho_SelectedContours.Dispose();
                ho_RegionLines.Dispose();
                ho_Plat.Dispose();
                ho_RegionBorder1.Dispose();
                ho_RegionClipped1.Dispose();
                ho_Skeleton1.Dispose();
                ho_Contours1.Dispose();
                ho_RegionLines1.Dispose();

                hv_row1.Dispose();
                hv_col1.Dispose();
                hv_row2.Dispose();
                hv_col2.Dispose();
                hv_RowBegin.Dispose();
                hv_ColBegin.Dispose();
                hv_RowEnd.Dispose();
                hv_ColEnd.Dispose();
                hv_Nr.Dispose();
                hv_Nc.Dispose();
                hv_Dist.Dispose();
                hv_RowBegin1.Dispose();
                hv_ColBegin1.Dispose();
                hv_RowEnd1.Dispose();
                hv_ColEnd1.Dispose();
                hv_Nr1.Dispose();
                hv_Nc1.Dispose();
                hv_Dist1.Dispose();
                hv_faRow1.Dispose();
                hv_faCol1.Dispose();
                hv_faRow2.Dispose();
                hv_faCol2.Dispose();
                hv_faRow3.Dispose();
                hv_faCol3.Dispose();
                hv_faRow4.Dispose();
                hv_faCol4.Dispose();
                hv_Angle.Dispose();
                hv_Deg.Dispose();
            }
        }        
    }
}

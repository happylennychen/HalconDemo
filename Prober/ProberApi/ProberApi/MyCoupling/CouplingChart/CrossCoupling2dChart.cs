using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ProberApi.MyCoupling.CouplingChart {
    public abstract class CrossCoupling2dChart {
        public const string LEGEND_TOP_MIDDLE = "TopMiddleLegend";

        public AutoResetEvent ChartLock { get; } = new AutoResetEvent(true);

        public CrossCoupling2dChart(Chart chart, Form parentForm, Action actActiveCross2DPage) {
            this.chart = chart;
            if (chart.Legends.FindByName(LEGEND_TOP_MIDDLE) == null) {
                chart.Legends.Add(new Legend(LEGEND_TOP_MIDDLE));
                chart.Legends[LEGEND_TOP_MIDDLE].Docking = Docking.Top;
                chart.Legends[LEGEND_TOP_MIDDLE].Alignment = StringAlignment.Center;
            }
            this.parentForm = parentForm;
            this.actActiveCross2DPage = actActiveCross2DPage;
        }

        public void BeginCoupling(string minValue) {
            actActiveCross2DPage.Invoke();

            ChartArea chartArea = new ChartArea();            
            parentForm.BeginInvoke(new Action(() => {
                chart.Series.Clear();
                chart.ChartAreas.Clear();
                
                chartArea.AxisX.Title = "Relative Distance";
                chartArea.AxisY.Title = "Feedback";
                chart.ChartAreas.Add(chartArea);
                chart.ChartAreas[0].AxisX.Minimum = Convert.ToDouble(minValue);
            }));
        }

        public virtual void AxisCouplingBegin(string seriesLegendText) {            
            parentForm.BeginInvoke(new Action(() => {
                currentSeries = new Series();
                currentSeries.Legend = LEGEND_TOP_MIDDLE;
                currentSeries.LegendText = seriesLegendText;
                currentSeries.ChartType = SeriesChartType.Line;
                chart.Series.Add(currentSeries);
            }));
        }
        
        public void AxisCouplingPeak(bool isShowPeakValue = false) {
            int peakIndex = feedbackList.IndexOf(feedbackList.Max());
            parentForm.BeginInvoke(new Action(() => {
                if (isShowPeakValue) {
                    DataPoint peakPoint = currentSeries.Points[peakIndex];
                    peakPoint.MarkerStyle = MarkerStyle.Diamond;
                    peakPoint.MarkerSize = 10;
                    peakPoint.MarkerColor = Color.Red;                
                    peakPoint.Label = peakPoint.YValues[0].ToString();
                }                
            }));
        }
        
        private readonly Chart chart;
        protected readonly Form parentForm;
        private readonly Action actActiveCross2DPage;
        protected Series currentSeries;
        protected List<double> feedbackList;
    }
}

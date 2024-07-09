using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ProberApi.MyCoupling.CouplingChart {
    public sealed class SpiralCoupling2dChart {
        public const string LEGEND_TOP_MIDDLE = "TopMiddleLegend";

        public AutoResetEvent ChartLock { get; } = new AutoResetEvent(true);

        public SpiralCoupling2dChart(Chart chart, Form parentForm, Action actActiveSpiral2DPage) {
            this.chart = chart;
            if (chart.Legends.FindByName(LEGEND_TOP_MIDDLE) == null) {
                chart.Legends.Add(new Legend(LEGEND_TOP_MIDDLE));
                chart.Legends[LEGEND_TOP_MIDDLE].Docking = Docking.Top;
                chart.Legends[LEGEND_TOP_MIDDLE].Alignment = StringAlignment.Center;
            }
            this.parentForm = parentForm;
            this.actActiveSpiral2DPage = actActiveSpiral2DPage;
        }

        public void BeginCoupling(string seriesLegendText) {
            feedbackList.Clear();
            actActiveSpiral2DPage.Invoke();
            ChartArea chartArea = new ChartArea();
            parentForm.BeginInvoke(new Action(() => {
                chart.Series.Clear();
                chart.ChartAreas.Clear();

                chartArea.AxisX.Title = "1st axis relative distance";
                chartArea.AxisY.Title = "2nd axis relative distance";
                chartArea.AxisX.Crossing = 0.0;
                chartArea.AxisY.Crossing = 0.0;
                chart.ChartAreas.Add(chartArea);

                currentSeries = new Series();
                currentSeries.Legend = LEGEND_TOP_MIDDLE;
                currentSeries.LegendText = seriesLegendText;
                currentSeries.ChartType = SeriesChartType.Bubble;                                                
                currentSeries.MarkerStyle = MarkerStyle.Circle;                                
                chart.Series.Add(currentSeries);
            }));
        }

        public void AddAPoint(double firstAxisRelDistance, double secondAxisRelDistance, double feedback) {
            feedbackList.Add(feedback);
            parentForm.BeginInvoke(new Action(() => {
                currentSeries.Points.AddXY(firstAxisRelDistance, secondAxisRelDistance, feedback);
            }));
        }

        public void EndCoupling() {
            int peakIndex = feedbackList.IndexOf(feedbackList.Max());
            parentForm.BeginInvoke(new Action(() => {
                DataPoint peakPoint = currentSeries.Points[peakIndex];
                peakPoint.MarkerStyle = MarkerStyle.Diamond;
                peakPoint.MarkerSize = 10;
                peakPoint.MarkerColor = Color.Red;
            }));
        }

        private readonly Chart chart;
        private readonly Form parentForm;
        private readonly Action actActiveSpiral2DPage;
        private Series currentSeries;
        private readonly List<double> feedbackList = new List<double>();
    }
}

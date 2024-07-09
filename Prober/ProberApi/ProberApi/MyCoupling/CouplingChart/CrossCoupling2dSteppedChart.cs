using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ProberApi.MyCoupling.CouplingChart {
    public sealed class CrossCoupling2dSteppedChart : CrossCoupling2dChart {
        public CrossCoupling2dSteppedChart(Chart chart, Form parentForm, Action actActiveCross2DPage) : base(chart, parentForm, actActiveCross2DPage) {
        }

        public override void AxisCouplingBegin(string seriesLegendText) {
            feedbackList = new List<double>();
            base.AxisCouplingBegin(seriesLegendText);            
        }

        public void AxisCouplingAddAPoint(double x, double feedback) {
            feedbackList.Add(feedback);
            parentForm.BeginInvoke(new Action(() => {
                currentSeries.Points.AddXY(x, feedback);
            }));
        }

        public void AxisCouplingContinuousDeclineFloor(int index) {
            parentForm.BeginInvoke(new Action(() => {
                DataPoint point = currentSeries.Points[index];
                point.MarkerStyle = MarkerStyle.Triangle;
                point.MarkerSize = 10;
                point.MarkerColor = Color.Purple;
            }));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ProberApi.MyCoupling.CouplingChart {
    public sealed class CrossCoupling2dTriggeredChart : CrossCoupling2dChart {
        public CrossCoupling2dTriggeredChart(Chart chart, Form parentForm, Action actActiveCross2DPage) : base(chart, parentForm, actActiveCross2DPage) {
        }

        public void AxisCouplingAddAllPoints(List<(double x, double feedback)> allPoints) {
            this.feedbackList = new List<double>();           
            foreach (var point in allPoints) {                
                feedbackList.Add(point.feedback);
                parentForm.BeginInvoke(new Action(() => {
                    currentSeries.Points.AddXY(point.x, point.feedback);
                }));
            }            
        }
    }
}

using CommonApi.MyUtility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProberApi.MyCoupling.CouplingDataHandling {
    public sealed class SingleAxisCouplingDataHandlingWeightedAverageMean : ISingleAxisCouplingDataHandling {
        public SingleAxisCouplingDataHandlingWeightedAverageMean(int pointNumber) {
            this.pointNumber = pointNumber;
            if (this.pointNumber < DEFAULT_POINT_NUMBER) {
                this.pointNumber = DEFAULT_POINT_NUMBER;
            }
            if (this.pointNumber % 2 == 0) {
                ++this.pointNumber;
            }
        }

        public SingleAxisCouplingDataHandlingWeightedAverageMean() {
            this.pointNumber = DEFAULT_POINT_NUMBER;
        }

        public (bool isOk, double peakAbsolutePosition) Handle(List<double> axisAbsolutePositionList, List<double> theFeedbackList) {
            List<double> feedbackList;
            if (IsFeedbackOpticsDbm(theFeedbackList)) {
                feedbackList = ConvertFeedbackDbmToMw(theFeedbackList);
            } else {
                feedbackList = theFeedbackList;
            }

            int peakIndex = feedbackList.IndexOf(feedbackList.Max());
            int pointNumberOneSide = (pointNumber - 1) / 2;

            int leftIndex = peakIndex - pointNumberOneSide;
            int rightIndex = peakIndex + pointNumberOneSide;
            if (leftIndex < 0 || rightIndex >= feedbackList.Count) {
                return (false, double.MinValue);
            }

            double weightSumOfFeedback = 0.0;
            double sumOfPositionMultiplyFeedback = 0.0;
            for (int i = leftIndex; i <= rightIndex; i++) {
                sumOfPositionMultiplyFeedback += axisAbsolutePositionList[i] * feedbackList[i];
                weightSumOfFeedback += feedbackList[i];
            }
            double weightedAverageMeanPosition = sumOfPositionMultiplyFeedback / weightSumOfFeedback;

            return (true, Math.Round(weightedAverageMeanPosition, 3));
        }

        //[gyh],2024-01-09，以下算法不严谨，开发时间不够，来不及思考严密方案。待后续其他同事完善！
        private bool IsFeedbackOpticsDbm(List<double> feedbackList) {
            foreach (double feedback in feedbackList) {
                if (feedback >= 0.0 || feedback < -100.0) {
                    return false;
                }
            }

            //double min = feedbackList.Min();
            //double max = feedbackList.Max();
            //if (Math.Abs(max - min) > 1) {
            //    return true;
            //} else {
            //    return false;
            //}

            return true;
        }

        private List<double> ConvertFeedbackDbmToMw(List<double> feedbackInDbmList) {
            List<double> mwList = new List<double>();
            foreach (double feedbackInDbm in feedbackInDbmList) {
                double feedbackInMw = MyStaticUtility.DbmToMw(feedbackInDbm);
                mwList.Add(feedbackInMw);
            }

            return mwList;
        }

        private readonly int pointNumber;
        private const int DEFAULT_POINT_NUMBER = 3;
    }
}

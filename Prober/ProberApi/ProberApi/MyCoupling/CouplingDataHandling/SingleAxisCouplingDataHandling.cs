using System.Collections.Generic;

namespace ProberApi.MyCoupling.CouplingDataHandling {
    public interface ISingleAxisCouplingDataHandling {        
        (bool isOk, double peakAbsolutePosition) Handle(List<double> axisAbsolutePositionList, List<double> feedbackList);
    }
}

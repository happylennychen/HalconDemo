using System.Collections.Generic;
using System.Linq;

using MyInstruments.MyUtility;

namespace ProberApi.MyUtility {
    public sealed class InstrumentUsageIdValidChecker {
        public InstrumentUsageIdValidChecker(List<InstrumentUsage> instrumentsUsages) { 
            this.instrumentsUsages = instrumentsUsages;
        }

        public bool Check(string instrumentUsageId) {
            var list = instrumentsUsages.Where(x => x.UsageId.Equals(instrumentUsageId)).ToList();
            return list.Any();
        }

        private readonly List<InstrumentUsage> instrumentsUsages;
    }
}

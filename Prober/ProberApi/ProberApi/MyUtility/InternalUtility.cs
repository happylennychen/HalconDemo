using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using MyInstruments;
using MyInstruments.MyUtility;

using ProberApi.MyConstant;

namespace ProberApi.MyUtility {
    internal sealed class InternalUtility {
        internal InternalUtility(ConcurrentDictionary<string, object> sharedObjects) {
            this.sharedObjects = sharedObjects;
            sharedObjects.TryGetValue(SharedObjectKey.INSTRUMENTS_USAGE_LIST, out object tempObj);
            this.instrumentsUsages = tempObj as List<InstrumentUsage>;
            sharedObjects.TryGetValue(SharedObjectKey.ALL_INSTRUMENTS, out tempObj);
            allInstruments = tempObj as Dictionary<string, Instrument>;

        }

        internal (bool isOk, Instrument instrument) GetInstrumentByUsageId(string instrumentUsageId) {
            var list = instrumentsUsages.Where(x => x.UsageId.Equals(instrumentUsageId)).ToList();
            if (list.Count == 0) {
                return (false, null);
            }
            InstrumentUsage instrumentUsage = list.First();
            Instrument instrument = allInstruments[instrumentUsage.InstrumentId];
            return (true, instrument);
        }

        internal readonly ConcurrentDictionary<string, object> sharedObjects;
        internal readonly List<InstrumentUsage> instrumentsUsages;
        internal readonly Dictionary<string, Instrument> allInstruments;
    }
}

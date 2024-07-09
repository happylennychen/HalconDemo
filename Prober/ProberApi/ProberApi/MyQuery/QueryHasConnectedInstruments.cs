using System.Collections.Concurrent;

using ProberApi.MyConstant;

namespace ProberApi.MyQuery {
    public sealed class QueryHasConnectedInstruments : AbstractQuery {
        public QueryHasConnectedInstruments(ConcurrentDictionary<string, object> sharedObjects) : base(sharedObjects) {
        }

        public override (bool isOk, string result) Query(string parameter) {
            sharedObjects.TryGetValue(SharedObjectKey.HAS_CONNECTED_INSTRUMENTS, out object tempObj);
            bool hasConnectedInstruments = (bool)tempObj;
            return (true, hasConnectedInstruments.ToString());
        }
    }
}

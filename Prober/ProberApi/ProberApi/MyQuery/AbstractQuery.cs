using System.Collections.Concurrent;

using CommonApi.MyUtility;
using NLog;

namespace ProberApi.MyQuery {
    public abstract class AbstractQuery {
        public AbstractQuery(ConcurrentDictionary<string, object> sharedObjects) {
            this.sharedObjects = sharedObjects;
        }        

        public abstract (bool isOk, string result) Query(string parameter);

        public string QueryType { get; protected set; }

        protected readonly ConcurrentDictionary<string, object> sharedObjects;
        protected static readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
    }
}

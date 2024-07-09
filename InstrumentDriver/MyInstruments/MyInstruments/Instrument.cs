using System.Collections.Generic;
using System.Threading;

using CommonApi.MyUtility;

using MyInstruments.MyEnum;

using NLog;

namespace MyInstruments {
    public abstract class Instrument {
        public abstract void Connect(string visaResource);
        public abstract void Disconnect();
        public abstract void ThrowExceptionWhenSlotOrChannelInvalid(string slot, string channel);        
        
        public string Id { get; private set; }
        public string Vendor { get; protected set; }
        public HashSet<string> SupportedModels { get; protected set; }
        public EnumInstrumentCategory IntrumentCategory { get; set; }
        public string CurrentModel { get; set; }
        public string VisaResource { get; set; }
        public AutoResetEvent TransactionLock {
            get {
                return autoResetEvent;
            }
        }

        public Instrument(string id) {
            this.Id = id;
        }

        private readonly AutoResetEvent autoResetEvent = new AutoResetEvent(true);
        protected static readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
    }
}

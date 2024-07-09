using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ProberApi.MyUtility {
    public sealed class MyCriticalSection {
        public MyCriticalSection(HashSet<AutoResetEvent> waitHandles) {
            this.waitHandles = waitHandles;
        }

        public void Enter() {
            if (waitHandles == null || waitHandles.Count == 0) {
                return;
            }

            while (!WaitHandle.WaitAll(waitHandles.ToArray(), TimeSpan.FromSeconds(1))) { }
        }

        public bool EnterWithTimeout(TimeSpan timeSpan) {
            return WaitHandle.WaitAll(waitHandles.ToArray(), timeSpan);
        }

        public void Leave() {
            if (waitHandles == null) {
                return;
            }

            foreach (var autoResetEvent in waitHandles) {
                autoResetEvent.Set();
            }
        }

        private readonly HashSet<AutoResetEvent> waitHandles;
    }
}

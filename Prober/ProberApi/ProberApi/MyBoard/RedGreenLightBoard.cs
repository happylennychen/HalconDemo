using System.Collections.Generic;

namespace ProberApi.MyBoard {
    public sealed class RedGreenLightBoard : Dictionary<string, bool> {
        public void AddOrUpdateLight(string key, bool value) {
            lock (this) {
                if (this.ContainsKey(key)) {
                    this[key] = value;
                } else {
                    this.Add(key, value);
                }
            }
        }

        public List<(string key, bool isGreen)> GetAll() {
            List<(string key, bool isGreen)> result = new List<(string key, bool isGreen)>();
            lock (this) {
                foreach (var one in this) {
                    result.Add((one.Key, one.Value));
                }

                return result;
            }
        }
    }
}

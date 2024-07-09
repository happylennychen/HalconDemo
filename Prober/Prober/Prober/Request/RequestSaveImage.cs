using MyInstruments.MyCamera;
using MyInstruments.MyUtility;
using MyInstruments;
using ProberApi.MyRequest;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProberApi.MyConstant;
using MyInstruments.MyEnum;
using CommonApi.MyEnum;
using CommonApi.MyUtility;

namespace Prober.Request {
    internal class RequestSaveImage : AbstractRequest {
        private readonly Dictionary<string, Instrument> instruments;
        private readonly List<InstrumentUsage> instrumentUsageList;
        private StandaloneCamera camera;

        public RequestSaveImage(ConcurrentDictionary<string, object> sharedObjects) : base(sharedObjects) {
            RequestType = PrivateRequestType.SAVE_IMAGE;

            sharedObjects.TryGetValue(SharedObjectKey.ALL_INSTRUMENTS, out object tempObj);
            instruments = tempObj as Dictionary<string, Instrument>;

            sharedObjects.TryGetValue(SharedObjectKey.INSTRUMENTS_USAGE_LIST, out tempObj);
            this.instrumentUsageList = tempObj as List<InstrumentUsage>;

            var getResult2 = GetInstrument("top_camera");
        }

        public (bool isOk, string errorText, Instrument instrument) GetInstrument(string instrumentUsageId) {
            string errorText = string.Empty;
            var list = this.instrumentUsageList.Where(x => x.UsageId.Equals(instrumentUsageId)).ToList();
            InstrumentUsage instrumentUsage = list.First();
            if (list == null) {
                errorText = $"GetInstrumentUsage(={instrumentUsageId}) does not exist!";
                LOGGER.Error(errorText);
                return (false, errorText, null);
            }

            Instrument instrument = instruments[instrumentUsage.InstrumentId];
            switch (instrumentUsage.InstrumentCategory) {
                case EnumInstrumentCategory.CCD:
                    camera = instrument as StandaloneCamera;
                    break;
                default:
                    errorText = $"{instrumentUsage.InstrumentCategory.ToString()} is not a valid instrument category of coupling feedback!";
                    LOGGER.Error(errorText);
                    return (false, errorText, null);
            }

            return (true, string.Empty, instrument);
        }

        public override (int responseId, string runResult, object attachedData) Run() {
            try {
                string fileSavePath = $"{this.imagePath}\\{this.imageName}.{this.imageFormat}";
                ImageFormat.TryParse(this.imageFormat, out ImageFormat temp);
                if (camera.SaveImage(fileSavePath, null, temp,this.compressLevel)) {
                    return ((int)EnumResponseId.PASS, string.Empty, null);
                } else {
                    return ((int)EnumResponseId.FAIL, "Save Image Failed", null);
                }
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return ((int)EnumResponseId.OCCURRED_EXCEPTION, ex.Message, null);
            }            
        }

        public override (bool isOk, string errorMessage) TryUpdateParameters(string parameters) {
            string INVALID_PARAMETERS = $"call RequestSaveImage.TryUpdateParameters({parameters})";

            string[] parts = parameters.Trim().Split(',');
            string errorText = string.Empty;
            if (parts.Length != 4) {
                errorText = $"{INVALID_PARAMETERS} --- parameter list should be: filePath,fileName,imageFormat!";
                LOGGER.Error(errorText);
                return (false, errorText);
            }

            this.imagePath = parts[0].Trim();
            this.imageName = parts[1].Trim();
            string format = parts[2].Trim();
            this.compressLevel = Convert.ToInt32(parts[3]);
            this.imageFormat = format.ToLower();
            if (!ImageFormat.TryParse(imageFormat, out ImageFormat temp)) {
                errorText = $"{INVALID_PARAMETERS} --- imageFormat(={format}) should be jpg or bmp only";
                LOGGER.Error(errorText);
                return (false, errorText);
            }

            return (true, string.Empty);
        }

        private string imagePath = string.Empty;
        private string imageName = string.Empty;
        private string imageFormat = string.Empty;
        private int compressLevel = 0;

        public override AbstractRequest DeepCopyDefaultInstance() {
            RequestSaveImage result = new RequestSaveImage(sharedObjects);
            return result;
        }
    }
}

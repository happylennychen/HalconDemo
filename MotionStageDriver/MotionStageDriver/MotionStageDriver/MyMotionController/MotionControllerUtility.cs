using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using CommonApi.MyUtility;

using MyMotionStageDriver.MyEnum;
using MyMotionStageDriver.MyMotionController.Leisai;
using MyMotionStageDriver.MyMotionController.Virtual;

using NLog;

namespace MyMotionStageDriver.MyMotionController {
    internal sealed class MotionControllerUtility {
        internal Dictionary<string, AbstractMotionController> MotionControllerDict { get; private set; } = new Dictionary<string, AbstractMotionController>();

        internal bool InitializeAllMotionControllers(bool isVirtualRunning, XElement xeMotionControllers) {
            MotionControllerDict.Clear();
            try {
                List<XElement> xeMotionControllerList = xeMotionControllers.Elements("motion_controller").ToList();
                foreach (XElement xeMotionController in xeMotionControllerList) {
                    string controllerTypeId = xeMotionController.Attribute("type_id").Value.Trim();
                    if (string.IsNullOrEmpty(controllerTypeId)) {
                        LOGGER.Error($"In config_motion_stages.xml, <motion_controllers><motion_controller type_id=>, type_id should not be empty!");
                        return false;
                    }
                    if (MotionControllerDict.ContainsKey(controllerTypeId)) {
                        LOGGER.Error($"In config_motion_stages.xml, <motion_controllers><motion_controller type_id={controllerTypeId}>, type_id(={controllerTypeId}) is duplicated!");
                        return false;
                    }
                    var initResult = InitializeMotionController(isVirtualRunning, xeMotionController);
                    if (!initResult.isOk) {
                        return false;
                    }

                    var motionController = initResult.motionController;
                    MotionControllerDict.Add(motionController.TypeId, motionController);
                }
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                return false;
            }

            return true;
        }

        private (bool isOk, AbstractMotionController motionController) InitializeMotionController(bool isVirtualRunning, XElement xeMotionController) {
            try {
                string typeId = xeMotionController.Attribute("type_id").Value.Trim();
                string vendor = xeMotionController.Attribute("vendor").Value.Trim();
                if (string.IsNullOrEmpty(vendor)) {
                    LOGGER.Error($"In config_motion_stages.xml, <motion_controllers><motion_controller type_id={typeId} vendor=>, vendor should not be empty!");
                    return (false, null);
                }
                string model = xeMotionController.Attribute("model").Value.Trim();
                if (string.IsNullOrEmpty(model)) {
                    LOGGER.Error($"In config_motion_stages.xml, <motion_controllers><motion_controller type_id={typeId} vendor={vendor} model=>, model should not be empty!");
                    return (false, null);
                }

                string errorTextVendorNotSupported = $"Motion controller vendor={vendor} does not be supported!";
                string errorTextModelNotSupported = $"Motion controller vendor={vendor}, model={model} does not be supported!";

                if (!Enum.TryParse(vendor, out EnumMotionControllerVendor controllerVendor)) {
                    LOGGER.Error(errorTextVendorNotSupported);
                    LOGGER.Error($"Please check config_motion_stages.xml, <motion_controllers><motion_controller type_id={typeId} vendor=? model=?>");
                    return (false, null);
                }

                AbstractMotionController motionController = null;
                switch (controllerVendor) {
                    case EnumMotionControllerVendor.LEISAI:
                        if (!Enum.TryParse(model, out EnumLeisaiModel leisaiModel)) {
                            LOGGER.Error(errorTextModelNotSupported);
                            LOGGER.Error($"Please check config_motion_stages.xml, <motion_controllers><motion_controller type_id={typeId} vendor=? model=?>");
                            return (false, null);
                        }
                        switch (leisaiModel) {
                            case EnumLeisaiModel.DMC3000:
                                if (isVirtualRunning) {
                                    motionController = new MotionControllerVirtual(typeId);
                                } else {
                                    motionController = new MotionControllerLeisaiDmc3000(typeId);
                                }
                                break;
                            default:
                                LOGGER.Error(errorTextModelNotSupported);
                                return (false, null);
                        }
                        break;
                    default:
                        LOGGER.Error(errorTextVendorNotSupported);
                        return (false, null);
                }

                if (!motionController.Initialize(xeMotionController)) {
                    LOGGER.Error($"Failed to initialize motion controller[typeId={typeId}]!");
                    return (false, null);
                }

                return (true, motionController);
            } catch (Exception ex) {
                LOGGER.Error($"Occurred exception: {ex.Message}");
                return (false, null);
            }
        }

        private static readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using HueModels = SmartHome.Models.PhilipsHue;
using BondModels = SmartHome.Models.Bond;
using SmartHome.Models.Contracts;
using SmartHome.ServerServices;

namespace SmartHome.Models
{
    public abstract class SmartContextBase
    {

        public SmartDevices Devices { get; }

        protected abstract IPhilipsHueClient PhilipsHueClient { get; }

        protected abstract IBondClient BondClient { get; }

        protected abstract ISmartThingsClient SmartThingsClient { get; }

        public abstract IScenesService Scenes { get; }

        public SmartContextBase()
        {
            Devices = new(this);
        }

        public async Task<IEnumerable<DeviceModelBase>> FetchAllDevicesAsync(CancellationToken cancellationToken = default)
        {
            var result = new List<DeviceModelBase>();

            var hueDevices = await PhilipsHueClient.GetAllLightsAsync(cancellationToken);
            result.AddRange(hueDevices);
            var motionDevices = await PhilipsHueClient.GetAllMotionSensorsAsync(cancellationToken);
            result.AddRange(motionDevices);

            var bondFans = await BondClient.GetCeilingFansAsync(cancellationToken);
            result.AddRange(bondFans);
            var bondRollers = await BondClient.GetRollersAsync(cancellationToken);
            result.AddRange(bondRollers);

            return result.OrderBy(r => r.Name).ToList();
        }

        public abstract Task<IEnumerable<DeviceLog>> GetListeningLogsAsync(int pageNumber,
                                                                          int pageSize,
                                                                          CancellationToken cancellationToken = default);


        #region Philips Hue

        public HueLightRequestModel Request(HueModels.LightModel model) => new(this, model.Id);

        public HueMotionRequestModel Request(HueModels.MotionModel model) => new(this, model.Id);

        public HueButtonRequestModel Request(HueModels.ButtonModel model) => new(this, model.Id);

        #endregion

        #region Bond

        public BondCeilingFanRequestModel Request(BondModels.CeilingFanModel model) => new(this, model.Id);

        public BondRollerRequestModel Request(BondModels.RollerModel model) => new(this, model.Id);


        #endregion


        #region Hue Request Models

        public abstract class HueRequestBase : RequestableDeviceBase
        {
            private readonly SmartContextBase _context;
            protected IPhilipsHueClient Client => _context.PhilipsHueClient;

            public HueRequestBase(SmartContextBase context, string id, DeviceType deviceType) : base(id, deviceType)
            {
                _context = context;
            }
        }

        public class HueMotionRequestModel : HueRequestBase
        {

            public HueMotionRequestModel(SmartContextBase context, string id) : base(context, id, DeviceType.HueMotion)
            {
            }

            public Task<HueModels.MotionModel> GetAsync(CancellationToken cancellationToken = default)
                => Client.GetMotionSensorAsync(Id, cancellationToken);

        }

        public class HueLightRequestModel : HueRequestBase
        {
            public HueLightRequestModel(SmartContextBase context, string id) : base(context, id, DeviceType.HueLight)
            {
            }

            public Task TriggerSwitchAsync(bool switchOn, CancellationToken cancellationToken = default)
                => Client.SwitchLightAsync(Id, switchOn, cancellationToken);

            public Task SetColorAsync(string colorHex, CancellationToken cancellationToken = default)
                => Client.SetLightColorAsync(Id, colorHex, cancellationToken);

            public Task<HueModels.LightModel> GetAsync(CancellationToken cancellationToken = default)
               => Client.GetLightAsync(Id, cancellationToken);

            public Task SetBrightnessAsync(double percent, CancellationToken cancellationToken = default)
                => Client.SetBrightnessAsync(Id, percent, cancellationToken);

        }

        public class HueButtonRequestModel : HueRequestBase
        {

            public HueButtonRequestModel(SmartContextBase context, string id) : base(context, id, DeviceType.HueButton)
            {
            }


            public Task<HueModels.ButtonModel> GetAsync(CancellationToken cancellationToken = default)
               => Client.GetButtonAsync(Id, cancellationToken);
        }

        public class HueRotaryRequestModel : HueRequestBase
        {
            public HueRotaryRequestModel(SmartContextBase context, string id) : base(context, id, DeviceType.HueRotary)
            {
            }

            public Task<HueModels.RotaryModel> GetAsync(CancellationToken cancellationToken = default)
                => Client.GetRotaryAsync(Id, cancellationToken);

        }

        #endregion


        #region Bond Request Models


        public abstract class BondRequestBase : RequestableDeviceBase
        {
            private readonly SmartContextBase _context;
            protected IBondClient Client => _context.BondClient;

            public BondRequestBase(SmartContextBase context, string id, DeviceType deviceType) : base(id, deviceType)
            {
                _context = context;
            }
        }

        public class BondCeilingFanRequestModel : BondRequestBase
        {

            public BondCeilingFanRequestModel(SmartContextBase source, string id) : base(source, id, DeviceType.BondFan)
            {
            }

            public Task<BondModels.CeilingFanModel> GetAsync(CancellationToken cancellationToken = default)
                => Client.GetCeilingFanAsync(Id, cancellationToken);

            public Task DecreaseAsync(CancellationToken cancellationToken = default)
                => Client.DecreaseFanAsync(Id, cancellationToken);

            public Task IncreaseAsync(CancellationToken cancellationToken = default)
                => Client.IncreaseFanAsync(Id, cancellationToken);

            public Task SwitchAsync(bool isOn, CancellationToken cancellationToken = default)
                => Client.SwitchFanAsync(Id, isOn, cancellationToken);

            public Task SwitchLightAsync(bool isOn, CancellationToken cancellationToken = default)
               => Client.SwitchFanLightAsync(Id, isOn, cancellationToken);

        }

        public class BondRollerRequestModel : BondRequestBase
        {

            public BondRollerRequestModel(SmartContextBase source, string id) : base(source, id, DeviceType.BondRoller)
            {
            }

            public Task<BondModels.RollerModel> GetAsync(CancellationToken cancellationToken = default)
                => Client.GetRollerAsync(Id, cancellationToken);

            public Task ToggleAsync(CancellationToken cancellationToken = default)
                => Client.ToggleRollerAsync(Id, cancellationToken);

        }

        #endregion

        #region Smartthings request models

        public abstract class SmartThingsRequestBase : RequestableDeviceBase
        {

            private readonly SmartContextBase _context;

            protected ISmartThingsClient Client => _context.SmartThingsClient;

            protected SmartThingsRequestBase(string id, DeviceType deviceType, SmartContextBase context) : base(id, deviceType)
            {
                _context = context;
            }

        }

        public class SwitchBotRequestModel : SmartThingsRequestBase
        {
            public SwitchBotRequestModel(SmartContextBase context, string id) : base(id, DeviceType.SwitchBot, context)
            {
            }

            public Task TriggerSwitchAsync(bool isOn, CancellationToken cancellationToken = default)
                => Client.TriggerSwitchBotAsync(Id, isOn, cancellationToken);

        }

        #endregion

    }
}

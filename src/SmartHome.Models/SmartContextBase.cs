﻿using SmartHome.Models.ClientContracts;
using SmartHome.ServerServices.Clients;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using HueModels = SmartHome.Models.PhilipsHue;
using BondModels = SmartHome.Models.Bond;

namespace SmartHome.Models
{
    public abstract class SmartContextBase
    {

        public SmartDevices Devices { get; }

        protected abstract IPhilipsHueClient PhilipsHueClient { get; }
        protected abstract IBondClient BondClient { get; }

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

        #endregion


        #region Bond

        public BondCeilingFanRequestModel Request(BondModels.CeilingFanModel model) => new(this, model.Id);

        public BondRollerRequestModel Request(BondModels.RollerModel model) => new(this, model.Id);



        #endregion

        #region Request Models

        public class HueMotionRequestModel : RequestableDeviceBase
        {

            private readonly IPhilipsHueClient _client;

            public HueMotionRequestModel(SmartContextBase context, string id) : base(id, DeviceType.HueMotion)
            {
                _client = context?.PhilipsHueClient;
            }

            public Task<HueModels.MotionModel> GetAsync(CancellationToken cancellationToken = default)
                => _client.GetMotionSensorAsync(Id, cancellationToken);

        }

        public class HueLightRequestModel : RequestableDeviceBase
        {
            private readonly IPhilipsHueClient _client;

            public HueLightRequestModel(SmartContextBase source, string id) : base(id, DeviceType.HueLight)
            {
                _client = source?.PhilipsHueClient;
            }

            public Task TriggerSwitchAsync(bool switchOn, CancellationToken cancellationToken = default)
                => _client.SwitchLightAsync(Id, switchOn, cancellationToken);

            public Task SetColorAsync(string colorHex, CancellationToken cancellationToken = default)
                => _client.SetLightColorAsync(Id, colorHex, cancellationToken);

            public Task<HueModels.LightModel> GetAsync(CancellationToken cancellationToken = default)
               => _client.GetLightAsync(Id, cancellationToken);



        }

        public class BondCeilingFanRequestModel : RequestableDeviceBase
        {
            private readonly IBondClient _client;


            public BondCeilingFanRequestModel(SmartContextBase source, string id) : base(id, DeviceType.BondFan)
            {
                _client = source?.BondClient;
            }

            public Task<BondModels.CeilingFanModel> GetAsync(CancellationToken cancellationToken = default)
                => _client.GetCeilingFanAsync(Id, cancellationToken);

            public Task DecreaseAsync(CancellationToken cancellationToken = default)
                => _client.DecreaseFanAsync(Id, cancellationToken);

            public Task IncreaseAsync(CancellationToken cancellationToken = default)
                => _client.IncreaseFanAsync(Id, cancellationToken);

        }

        public class BondRollerRequestModel : RequestableDeviceBase
        {
            private readonly IBondClient _client;

            public BondRollerRequestModel(SmartContextBase source, string id) : base(id, DeviceType.BondRoller)
            {
                _client = source?.BondClient;
            }

            public Task<BondModels.RollerModel> GetAsync(CancellationToken cancellationToken = default)
                => _client.GetRollerAsync(Id, cancellationToken);

        }

        #endregion

    }
}

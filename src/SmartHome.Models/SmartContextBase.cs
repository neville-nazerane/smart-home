using SmartHome.Models.ClientContracts;
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

        protected abstract IPhilipsHueClient PhilipsHueClient { get; }
        protected abstract IBondClient BondClient { get; }


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

        #region Philips Hue

        public HueLightRequestModel MakeRequest(HueModels.LightModel model) => new(this, model.Id);

        public HueMotionRequestModel MakeRequest(HueModels.MotionModel model) => new(this, model.Id);

        #endregion


        public BondCeilingFanRequestModel MakeRequest(BondModels.CeilingFanModel model) => new(this, model.Id);

        public BondRollerRequestModel MakeRequest(BondModels.RollerModel model) => new(this, model.Id);


        #region Request Models

        public class HueMotionRequestModel
        {

            private readonly IPhilipsHueClient _client;

            public string Id { get; }

            public HueMotionRequestModel(SmartContextBase context, string id)
            {
                _client = context.PhilipsHueClient;
                Id = id;
            }

            public Task<HueModels.MotionModel> GetAsync(CancellationToken cancellationToken = default)
                => _client.GetMotionSensorAsync(Id, cancellationToken);

        }

        public class HueLightRequestModel
        {
            private readonly IPhilipsHueClient _client;

            public string Id { get; }

            public HueLightRequestModel(SmartContextBase source, string id)
            {
                _client = source.PhilipsHueClient;
                Id = id;
            }

            public Task TriggerSwitchAsync(bool switchOn, CancellationToken cancellationToken = default)
                => _client.SwitchLightAsync(Id, switchOn, cancellationToken);

            public Task<HueModels.LightModel> GetAsync(CancellationToken cancellationToken = default)
               => _client.GetLightAsync(Id, cancellationToken);

        }

        public class BondCeilingFanRequestModel
        {
            private readonly IBondClient _client;

            public string Id { get; }

            public BondCeilingFanRequestModel(SmartContextBase source, string id)
            {
                _client = source.BondClient;
                Id = id;
            }

            public Task<BondModels.CeilingFanModel> GetAsync(CancellationToken cancellationToken = default)
                => _client.GetCeilingFanAsync(Id, cancellationToken);

        }

        public class BondRollerRequestModel
        {
            private readonly IBondClient _client;

            public string Id { get; }

            public BondRollerRequestModel(SmartContextBase source, string id)
            {
                _client = source.BondClient;
                Id = id;
            }

            public Task<BondModels.RollerModel> GetAsync(CancellationToken cancellationToken = default)
                => _client.GetRollerAsync(Id, cancellationToken);

        }

        #endregion

    }
}

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

        public HueLightRequestModel MakeRequest(HueModels.LightModel model) => MakeHueLightRequestModel(model.Id);
        public HueLightRequestModel MakeHueLightRequestModel(string id) => new(this, id);

        public HueMotionRequestModel MakeRequest(HueModels.MotionModel model) => MakeHueMotionRequestModel(model.Id);
        public HueMotionRequestModel MakeHueMotionRequestModel(string id) => new(this, id);

        #endregion



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

        #endregion

    }
}

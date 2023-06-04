﻿using SmartHome.Models.ClientContracts;
using System;
using System.Collections.Generic;
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

        //protected abstract IEnumerable<HueModels.LightModel> GetAllPhilipsHueLightsAsync();

        public async Task<IEnumerable<DeviceModelBase>> FetchAllDevicesAsync()
        {
            var result = new List<DeviceModelBase>();

            var hueDevices = await PhilipsHueClient.GetAllLightsAsync();
            result.AddRange(hueDevices);

            return result.OrderBy(r => r.Name).ToList();
        }

        #region Philips Hue

        public HueLightRequestModel MakeRequest(HueModels.LightModel model) => MakeHueLightRequestModel(model.Id);
        public HueLightRequestModel MakeHueLightRequestModel(string id) => new(this, id);

        #endregion


        #region Request Models

        public class HueLightRequestModel
        {
            private readonly IPhilipsHueClient _client;

            public string Id { get; set; }

            [JsonConstructor]
            public HueLightRequestModel()
            {
                
            }

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

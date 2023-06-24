using SmartHome.Models;
using HueModels = SmartHome.Models.PhilipsHue;
using BondModels = SmartHome.Models.Bond;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Diagnostics;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Runtime.CompilerServices;
using SmartHome.Models.Contracts;
using SmartHome.ServerServices;

namespace SmartHome.ClientServices
{
    public class SmartContext : SmartContextBase
    {
        private readonly AllCLients _allClients;
        private readonly HttpClient _httpClient;

        protected override IPhilipsHueClient PhilipsHueClient => _allClients;
        protected override IBondClient BondClient => _allClients;
        public override IScenesService Scenes => _allClients;

        protected override ISmartThingsClient SmartThingsClient => _allClients;

        public SmartContext(HttpClient httpClient) : base()
        {
            _allClients = new AllCLients(httpClient);
            _httpClient = httpClient;
        }

        public override Task<IEnumerable<DeviceLog>> GetListeningLogsAsync(int pageNumber,
                                                                                int pageSize,
                                                                                CancellationToken cancellationToken = default)
            => _httpClient.GetFromJsonAsync<IEnumerable<DeviceLog>>($"listeningLogs?pageNumber={pageNumber}&pageSize={pageSize}", cancellationToken);


        class AllCLients : IPhilipsHueClient, IBondClient, IScenesService, ISmartThingsClient
        {
            private readonly HttpClient _httpClient;

            public AllCLients(HttpClient httpClient)
            {
                _httpClient = httpClient;
            }

            public Task<IEnumerable<HueModels.LightModel>> GetAllLightsAsync(CancellationToken cancellationToken = default)
                => _httpClient.GetFromJsonAsync<IEnumerable<HueModels.LightModel>>("philipsHue/lights", cancellationToken);

            public Task<HueModels.LightModel> GetLightAsync(string id, CancellationToken cancellationToken = default)
                => _httpClient.GetFromJsonAsync<HueModels.LightModel>($"philipsHue/light/{id}", cancellationToken);

            public Task<IEnumerable<HueModels.MotionModel>> GetAllMotionSensorsAsync(CancellationToken cancellationToken = default)
                => _httpClient.GetFromJsonAsync<IEnumerable<HueModels.MotionModel>>("philipsHue/motions", cancellationToken);

            public Task<HueModels.MotionModel> GetMotionSensorAsync(string id, CancellationToken cancellationToken = default)
                => _httpClient.GetFromJsonAsync<HueModels.MotionModel>($"philipsHue/motion/{id}", cancellationToken);

            public async Task SwitchLightAsync(string id, bool switchOn, CancellationToken cancellationToken = default)
            {
                using var res = await _httpClient.PutAsync($"philipsHue/switchLight/{id}/{switchOn}", null, cancellationToken);
                res.EnsureSuccessStatusCode();
            }

            public Task<IEnumerable<BondModels.CeilingFanModel>> GetCeilingFansAsync(CancellationToken cancellationToken = default)
                => _httpClient.GetFromJsonAsync<IEnumerable<BondModels.CeilingFanModel>>("bond/ceilingFans", cancellationToken);

            public Task<IEnumerable<BondModels.RollerModel>> GetRollersAsync(CancellationToken cancellationToken = default)
                => _httpClient.GetFromJsonAsync<IEnumerable<BondModels.RollerModel>>("bond/rollers", cancellationToken);

            public Task<BondModels.CeilingFanModel> GetCeilingFanAsync(string id, CancellationToken cancellationToken = default)
                => _httpClient.GetFromJsonAsync<BondModels.CeilingFanModel>($"bond/ceilingFan/{id}", cancellationToken);

            public Task<BondModels.RollerModel> GetRollerAsync(string id, CancellationToken cancellationToken = default)
                => _httpClient.GetFromJsonAsync<BondModels.RollerModel>($"bond/roller/{id}", cancellationToken);


            public async Task TurnOffFanAsync(string id, CancellationToken cancellationToken = default)
            {
                using var res = await _httpClient.PutAsync($"bond/ceilingFan/{id}/off", null, cancellationToken);
                res.EnsureSuccessStatusCode();
            }

            public async Task TurnOffFanLightAsync(string id, CancellationToken cancellationToken = default)
            {
                using var res = await _httpClient.PutAsync($"bond/ceilingFan/{id}/lightOff", null, cancellationToken);
                res.EnsureSuccessStatusCode();
            }

            public async Task TurnOnFanAsync(string id, CancellationToken cancellationToken = default)
            {
                using var res = await _httpClient.PutAsync($"bond/ceilingFan/{id}/lightOn", null, cancellationToken);
                res.EnsureSuccessStatusCode();
            }

            public async Task TurnOnFanLightAsync(string id, CancellationToken cancellationToken = default)
            {
                using var res = await _httpClient.PutAsync($"bond/ceilingFan/{id}/on", null, cancellationToken);
                res.EnsureSuccessStatusCode();
            }

            public async Task DecreaseFanAsync(string id, CancellationToken cancellationToken = default)
            {
                using var res = await _httpClient.PutAsync($"bond/ceilingFan/{id}/decrease", null, cancellationToken);
                res.EnsureSuccessStatusCode();
            }

            public async Task IncreaseFanAsync(string id, CancellationToken cancellationToken = default)
            {
                using var res = await _httpClient.PutAsync($"bond/ceilingFan/{id}/increase", null, cancellationToken);
                res.EnsureSuccessStatusCode();
            }

            public async Task SetLightColorAsync(string id, string colorHex, CancellationToken cancellationToken = default)
            {
                colorHex = colorHex.Replace("#", string.Empty);
                using var res = await _httpClient.PutAsync($"philipsHue/color/{id}/{colorHex}", null, cancellationToken);
                res.EnsureSuccessStatusCode();
            }

            public Task<HueModels.ButtonModel> GetButtonAsync(string id, CancellationToken cancellationToken = default)
                => _httpClient.GetFromJsonAsync<HueModels.ButtonModel>($"philipsHue/button/{id}", cancellationToken);

            public Task<IEnumerable<Scene>> GetAllScenesAsync(CancellationToken cancellationToken = default)
                => _httpClient.GetFromJsonAsync<IEnumerable<Scene>>("scenes", cancellationToken);


            public async Task<bool> IsAnySceneEnabledAsync(params SceneName[] sceneNames)
            {
                using var res = await _httpClient.PostAsJsonAsync("scene/anyEnabled", sceneNames);
                res.EnsureSuccessStatusCode();
                return await res.Content.ReadFromJsonAsync<bool>();
            }

            public Task<bool> IsEnabledAsync(SceneName sceneName, CancellationToken cancellationToken = default)
                => _httpClient.GetFromJsonAsync<bool>($"scene/{sceneName}", cancellationToken);

            public async Task SetSceneEnabledAsync(SceneName sceneName, bool isEnabled, CancellationToken cancellationToken = default)
            {
                using var res = await _httpClient.PutAsync($"scene/{sceneName}/{isEnabled}", null, cancellationToken);
                res.EnsureSuccessStatusCode();
            }

            public async Task SwitchAsync(SceneName sceneName, CancellationToken cancellationToken = default)
            {
                using var res = await _httpClient.PutAsync($"scene/{sceneName}/switch", null, cancellationToken);
                res.EnsureSuccessStatusCode();
            }

            public async Task TriggerSwitchBotAsync(string deviceId, bool isOn, CancellationToken cancellationToken = default)
            {
                using var res = await _httpClient.PostAsync($"smartthings/switchbot/{deviceId}/trigger/{isOn}", null, cancellationToken);
                res.EnsureSuccessStatusCode();
            }

        }


    }
}

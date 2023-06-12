using SmartHome.Models;
using SmartHome.Models.ClientContracts;
using HueModels = SmartHome.Models.PhilipsHue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using SmartHome.ServerServices.Clients;
using SmartHome.Models.Bond;

namespace SmartHome.ClientServices
{
    public class SmartContext : SmartContextBase
    {
        private readonly AllCLients _allClients;

        protected override IPhilipsHueClient PhilipsHueClient => _allClients;
        protected override IBondClient BondClient => _allClients;

        public SmartContext(HttpClient httpClient) : base()
        {
            _allClients = new AllCLients(httpClient);
        }


        class AllCLients : IPhilipsHueClient, IBondClient
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

            public Task<IEnumerable<CeilingFanModel>> GetCeilingFansAsync(CancellationToken cancellationToken = default)
                => _httpClient.GetFromJsonAsync<IEnumerable<CeilingFanModel>>("bond/ceilingFans", cancellationToken);

            public Task<IEnumerable<RollerModel>> GetRollersAsync(CancellationToken cancellationToken = default)
                => _httpClient.GetFromJsonAsync<IEnumerable<RollerModel>>("bond/rollers", cancellationToken);

            public Task<CeilingFanModel> GetCeilingFanAsync(string id, CancellationToken cancellationToken = default)
                => _httpClient.GetFromJsonAsync<CeilingFanModel>($"bond/ceilingFan/{id}", cancellationToken);

            public Task<RollerModel> GetRollerAsync(string id, CancellationToken cancellationToken = default)
                => _httpClient.GetFromJsonAsync<RollerModel>($"bond/roller/{id}", cancellationToken);

            public Task DecreaseFanAsync(string id, CancellationToken cancellationToken = default)
                => _httpClient.PutAsync($"bond/roller/{id}/decrease", null, cancellationToken);

            public Task IncreaseFanAsync(string id, CancellationToken cancellationToken = default)
                => _httpClient.PutAsync($"bond/roller/{id}/increase", null, cancellationToken);

        }

    }
}

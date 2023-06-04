using SmartHome.Models;
using SmartHome.Models.ClientContracts;
using HueModels = SmartHome.Models.PhilipsHue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace SmartHome.ClientServices
{
    public class SmartContext : SmartContextBase
    {
        private readonly HttpClient _httpClient;
        private readonly AllCLients _allClients;

        protected override IPhilipsHueClient PhilipsHueClient => _allClients;

        public SmartContext(HttpClient httpClient) : base()
        {
            _httpClient = httpClient;
            _allClients = new AllCLients(httpClient);
        }


        class AllCLients : IPhilipsHueClient
        {
            private readonly HttpClient _httpClient;

            public AllCLients(HttpClient httpClient)
            {
                _httpClient = httpClient;
            }

            public Task<IEnumerable<HueModels.LightModel>> GetAllLightsAsync(CancellationToken cancellationToken = default)
                => _httpClient.GetFromJsonAsync<IEnumerable<HueModels.LightModel>>("philipsHue/lights", cancellationToken);

            public async Task SwitchLightAsync(HueLightRequestModel request, bool switchOn, CancellationToken cancellationToken = default)
            {
                using var res = await _httpClient.PutAsync($"philipsHue/switchLight/{request.Id}/{switchOn}", null, cancellationToken);
                res.EnsureSuccessStatusCode();
            }
        }

    }
}

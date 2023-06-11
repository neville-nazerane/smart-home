using SmartHome.Models;
using SmartHome.Models.Bond;
using SmartHome.ServerServices.InternalJsonConverters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHome.ServerServices.Clients
{
    public class BondClient : IBondClient
    {
        private readonly HttpClient _client;

        private static readonly JsonSerializerOptions DeviceIdsJsonOptions;

        static BondClient()
        {
            DeviceIdsJsonOptions = new();
            DeviceIdsJsonOptions.Converters.Add(new BondDeviceIdsJsonConverter());
        }

        public BondClient(HttpClient client)
        {
            _client = client;
        }

        public static HttpClient SetupClient(HttpClient client,
                                             string baseUrl,
                                             string token)
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Add("BOND-Token", token);
            return client;
        }

        public async Task<IEnumerable<CeilingFanModel>> GetCeilingFansAsync(CancellationToken cancellationToken = default)
        {
            var result = new List<CeilingFanModel>();
            var infos = GetAllDeviceInfosAsync(cancellationToken);
            await foreach (var info in infos)
            {
                if (info.Type == "CF")
                {
                    var model = FillUpBaseModel(info, new CeilingFanModel());
                    await FillupCFanStateAsync(model, cancellationToken);
                    result.Add(model);
                }
            }
            return result;
        }

        public async Task<IEnumerable<RollerModel>> GetRollersAsync(CancellationToken cancellationToken = default)
        {
            var result = new List<RollerModel>();
            var infos = GetAllDeviceInfosAsync(cancellationToken);
            await foreach (var info in infos)
            {
                if (info.Type == "MS")
                {
                    var model = FillUpBaseModel(info, new RollerModel());
                    await FillupRollerStateAsync(model, cancellationToken);
                    result.Add(model);
                }
            }
            return result;
        }

        async IAsyncEnumerable<DeviceInfo> GetAllDeviceInfosAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var deviceIds = await _client.GetFromJsonAsync<List<string>>("v2/devices", DeviceIdsJsonOptions, cancellationToken);
            foreach (var deviceId in deviceIds)
            {
                var info = await _client.GetFromJsonAsync<DeviceInfo>($"v2/devices/{deviceId}", cancellationToken);
                info.Id = deviceId;
                yield return info;
            }
        }

        async Task FillupCFanStateAsync(CeilingFanModel fan, CancellationToken cancellationToken = default)
        {
            var state = await _client.GetFromJsonAsync<CFanState>($"v2/devices/{fan.Id}/state", cancellationToken);
            fan.Speed = (short)(state.Power * state.Speed);
        }

        async Task FillupRollerStateAsync(RollerModel roller, CancellationToken cancellationToken = default)
        {
            var state = await _client.GetFromJsonAsync<RollerState>($"v2/devices/{roller.Id}/state", cancellationToken);
            roller.IsOpen = state.Open == 1;
        }


        static TModel FillUpBaseModel<TModel>(DeviceInfo info, TModel model)
            where TModel : DeviceModelBase
        {
            model.Id = info.Id;
            model.Name = info.Name;
            return model;
        }


        class RollerState
        {
            [JsonPropertyName("open")]
            public byte Open { get; set; }

        }

        class CFanState
        {
            [JsonPropertyName("power")]
            public int Power { get; set; }

            [JsonPropertyName("speed")]
            public int Speed { get; set; }

            [JsonPropertyName("light")]
            public int Light { get; set; }
        }

        class DeviceInfo
        {

            [JsonIgnore]
            public string Id { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("type")]
            public string Type { get; set; }

        }

    }

}

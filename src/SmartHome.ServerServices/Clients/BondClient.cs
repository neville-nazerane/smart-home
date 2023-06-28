using SmartHome.Models;
using SmartHome.Models.Bond;
using SmartHome.Models.Contracts;
using SmartHome.ServerServices.InternalJsonConverters;
using SmartHome.ServerServices.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net.NetworkInformation;
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

        public string GetIp() => _client.BaseAddress.Host;


        #region Fan

        public async Task<CeilingFanModel> GetCeilingFanAsync(string id, CancellationToken cancellationToken = default)
        {
            var info = await GetDeviceInfoAsync(id, cancellationToken);
            var fan = FillUpBaseModel(info, new CeilingFanModel());
            await FillupCFanStateAsync(fan, cancellationToken);
            return fan;
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

        async Task FillupCFanStateAsync(CeilingFanModel fan, CancellationToken cancellationToken = default)
        {
            var state = await GetFanStateAsync(fan.Id, cancellationToken);
            fan.Speed = (short)(state.Power * state.Speed);
            fan.LightIsOn = state.Light == 1;
        }

        public async Task DecreaseFanAsync(string id, CancellationToken cancellationToken = default)
        {
            var state = await GetFanStateAsync(id, cancellationToken);
            int targetSpeed = (state.Power * state.Speed) - 1;
            if (targetSpeed == 0)
                await RunActionAsync(id, "TurnOff", cancellationToken);
            else if (targetSpeed > 0)
            {
                var model = new BondRequest
                {
                    Argument = (short)targetSpeed
                };
                await RunActionAsync(id, "SetSpeed", model, cancellationToken);
            }
        }

        public async Task IncreaseFanAsync(string id, CancellationToken cancellationToken = default)
        {
            var state = await GetFanStateAsync(id, cancellationToken);
            int targetSpeed = (state.Power * state.Speed) + 1;
            if (targetSpeed < 4)
            {
                var model = new BondRequest
                {
                    Argument = (short)targetSpeed
                };
                await RunActionAsync(id, "SetSpeed", model, cancellationToken);
            }
        }

        public Task SwitchFanAsync(string id, bool isOn, CancellationToken cancellationToken = default)
            => RetryUtil.Setup(4, TimeSpan.FromMilliseconds(200))
                        .SetVerification(async () =>
                        {
                            var status = await GetFanStateAsync(id);
                            return status.Power == 1 == isOn;
                        })
                        .ExecuteAsync(() => RunActionAsync(id, isOn ? "TurnOn" : "TurnOff", cancellationToken));

        public Task SwitchFanLightAsync(string id, bool isOn, CancellationToken cancellationToken = default)
            => RetryUtil.Setup(4, TimeSpan.FromMilliseconds(200))
                        .SetVerification(async () =>
                        {
                            var status = await GetFanStateAsync(id);
                            return status.Light == 1 == isOn;
                        })
                        .ExecuteAsync(() => RunActionAsync(id, isOn ? "TurnLightOn" : "TurnLightOff", cancellationToken));


        Task<CFanState> GetFanStateAsync(string id, CancellationToken cancellationToken = default) => _client.GetFromJsonAsync<CFanState>($"v2/devices/{id}/state", cancellationToken);

        public async Task<DeviceType> GetDeviceTypeAsync(string id, CancellationToken cancellationToken = default)
        {
            var info = await GetDeviceInfoAsync(id, cancellationToken);
            return info.Type switch
            {
                "CF" => DeviceType.BondFan,
                "MS" => DeviceType.BondRoller,
                _ => DeviceType.None,
            };
        }

        #endregion

        #region Roller

        public async Task<RollerModel> GetRollerAsync(string id, CancellationToken cancellationToken = default)
        {
            var info = await GetDeviceInfoAsync(id, cancellationToken);
            var roller = FillUpBaseModel(info, new RollerModel());
            await FillupRollerStateAsync(roller, cancellationToken);
            return roller;
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

        async Task FillupRollerStateAsync(RollerModel roller, CancellationToken cancellationToken = default)
        {
            var state = await _client.GetFromJsonAsync<RollerState>($"v2/devices/{roller.Id}/state", cancellationToken);
            roller.IsOpen = state.Open == 1;
        }

        public Task ToggleRollerAsync(string id, CancellationToken cancellationToken = default)
            => RunActionAsync(id, "ToggleOpen", cancellationToken);

        #endregion

        Task RunActionAsync(string deviceId, string action, CancellationToken cancellationToken = default)
            => RunActionCoreAsync(deviceId, action, "{}", cancellationToken);

        Task RunActionAsync<TData>(string deviceId, string action, TData data, CancellationToken cancellationToken = default)
            => RunActionCoreAsync(deviceId, action, JsonSerializer.Serialize(data), cancellationToken);



        async Task RunActionCoreAsync(string deviceId, string action, string data, CancellationToken cancellationToken = default)
        {
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            using var res = await _client.PutAsync($"/v2/devices/{deviceId}/actions/{action}", content, cancellationToken);
            res.EnsureSuccessStatusCode();
        }

        async IAsyncEnumerable<DeviceInfo> GetAllDeviceInfosAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var deviceIds = await _client.GetFromJsonAsync<List<string>>("v2/devices", DeviceIdsJsonOptions, cancellationToken);
            foreach (var deviceId in deviceIds)
            {
                var info = await GetDeviceInfoAsync(deviceId, cancellationToken);
                yield return info;
            }
        }

        private async Task<DeviceInfo> GetDeviceInfoAsync(string deviceId, CancellationToken cancellationToken)
        {
            var res = await _client.GetFromJsonAsync<DeviceInfo>($"v2/devices/{deviceId}", cancellationToken);
            res.Id = deviceId;
            return res;
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

        class BondRequest
        {
            [JsonPropertyName("argument")]
            public short Argument { get; set; }
        }

        class CFanState
        {
            [JsonPropertyName("power")]
            public byte Power { get; set; }

            [JsonPropertyName("speed")]
            public int Speed { get; set; }

            [JsonPropertyName("light")]
            public byte Light { get; set; }
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

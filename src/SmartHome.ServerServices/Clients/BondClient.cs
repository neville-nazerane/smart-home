﻿using SmartHome.Models;
using SmartHome.Models.Bond;
using SmartHome.ServerServices.InternalJsonConverters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHome.ServerServices.Clients
{
    public class BondClient
    {
        private readonly HttpClient _client;

        private static readonly JsonSerializerOptions DeviceIdsJsonOptions;

        static BondClient()
        {
            DeviceIdsJsonOptions = new();
            DeviceIdsJsonOptions.Converters.Add(new BondDeviceIds());
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

        private async Task<IEnumerable<string>> GetAllDeviceIdsAsync(CancellationToken cancellationToken = default)
            => await _client.GetFromJsonAsync<List<string>>("v2/devices", DeviceIdsJsonOptions, cancellationToken);

        public async Task<IEnumerable<object>> GetAllDevicesAsync(CancellationToken cancellationToken = default)
        {
            var result = new List<object>();
            var deviceIds = await _client.GetFromJsonAsync<List<string>>("v2/devices", DeviceIdsJsonOptions, cancellationToken);
            foreach (var deviceId in deviceIds)
            {
                var info = await _client.GetFromJsonAsync<DeviceInfo>($"v2/devices/{deviceId}", cancellationToken);
                void FillUpBaseModel(DeviceModelBase model)
                {
                    model.Id = deviceId;
                    model.Name = info.Name;
                }
                switch (info.Type)
                {
                    case "CF":
                        var fan = new CeilingFanModel();
                        FillUpBaseModel(fan);
                        await FillupCFanStateAsync(fan, cancellationToken);
                        result.Add(fan);
                        break;

                    case "MS":
                        var roller = new RollerModel();
                        FillUpBaseModel(roller);
                        await FillupRollerStateAsync(roller, cancellationToken);
                        result.Add(roller);
                        break;
                }
            }

            return result;
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

            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("type")]
            public string Type { get; set; }

        }

    }

}

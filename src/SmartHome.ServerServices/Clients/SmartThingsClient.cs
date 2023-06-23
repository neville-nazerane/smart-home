using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.ServerServices.Clients
{
    public class SmartThingsClient : ISmartThingsClient
    {
        private readonly HttpClient _httpClient;

        public SmartThingsClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public static void SetupClient(HttpClient client, string pat)
        {
            client.BaseAddress = new Uri("https://api.smartthings.com/v1");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", pat);
            client.Timeout = TimeSpan.FromSeconds(1);
        }

        public Task<bool> ExecuteDeviceAsync(string deviceId, DeviceExecuteModel model, CancellationToken cancellationToken = default)
            => ExecuteDeviceAsync(deviceId, new DeviceExecuteModel[] { model }, cancellationToken);

        public async Task<bool> ExecuteDeviceAsync(string deviceId, DeviceExecuteModel[] models, CancellationToken cancellationToken = default)
        {
            try
            {
                using var res = await _httpClient.PostAsJsonAsync($"/devices/{deviceId}/commands", new { commands = models }, cancellationToken);
                res.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                if (ex is HttpRequestException) throw;
                return false;
            }
            return true;
        }

        public class DeviceExecuteModel
        {
            public string Component { get; set; }
            public string Capability { get; set; }

            public string Command { get; set; }
            public object[] Arguments { get; set; }

        }

    }
}

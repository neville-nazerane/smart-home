using SmartHome.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.BackgroundProcessor.Services
{
    public class ApiConsumer
    {
        private readonly HttpClient _client;

        public ApiConsumer(HttpClient client)
        {
            _client = client;
        }

        public async Task NotifyDeviceChangeAsync(DeviceChangedNotify device, 
                                                  CancellationToken cancellationToken = default)
        {
            using var res = await _client.PostAsJsonAsync("notifyDeviceChange", device, cancellationToken);
            res.EnsureSuccessStatusCode();
        }

    }
}

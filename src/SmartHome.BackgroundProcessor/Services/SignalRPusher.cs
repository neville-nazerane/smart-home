using SmartHome.Models;
using SmartHome.ServerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.BackgroundProcessor.Services
{
    public class SignalRPusher : ISignalRPusher
    {
        private readonly HttpClient _client;

        public SignalRPusher(HttpClient client)
        {
            _client = client;
        }

        public async Task NotifyDeviceChangeAsync(ListenedDevice device, 
                                                  CancellationToken cancellationToken = default)
        {
            using var res = await _client.PostAsJsonAsync("notifyDeviceChange", device, cancellationToken);
            res.EnsureSuccessStatusCode();
        }

        public async Task NotifySceneChangeAsync(Scene scene, CancellationToken cancellationToken = default)
        {
            using var res = await _client.PostAsJsonAsync("notifySceneChange", scene, cancellationToken);
            res.EnsureSuccessStatusCode();
        }
    }
}

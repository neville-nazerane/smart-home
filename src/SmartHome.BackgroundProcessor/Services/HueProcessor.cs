using SmartHome.Models.ClientContracts;
using SmartHome.Models.PhilipsHue;
using SmartHome.ServerServices.Clients;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHome.BackgroundProcessor.Services
{
    internal class HueProcessor
    {
        private readonly ConcurrentQueue<HttpResponseMessage> _queue;
        private readonly ILogger<HueProcessor> _logger;
        private readonly IPhilipsHueClient _philipsHueClient;

        public HueProcessor(ILogger<HueProcessor> logger, IPhilipsHueClient philipsHueClient)
        {
            _logger = logger;
            _philipsHueClient = philipsHueClient;

            _queue = new();
        }

        public async Task KeepListeningAsync(CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var res = await _philipsHueClient.StreamEventAsync(cancellationToken);
                    _queue.Enqueue(res);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed single listen");
                }
            }
        }

        public async Task ProcessQueueAsync(CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    if (_queue.TryDequeue(out var response))
                    {
                        var data = await response.Content.ReadFromJsonAsync<IEnumerable<HueEvent>>(cancellationToken: cancellationToken);
                        response.Dispose();

                    }
                    else await Task.Delay(1000, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed single enqueue");
                }
            }
        }



    }
}

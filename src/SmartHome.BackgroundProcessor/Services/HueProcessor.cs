using SmartHome.BackgroundProcessor.Util;
using SmartHome.Models;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SmartHome.BackgroundProcessor.Services
{
    internal class HueProcessor
    {
        private readonly ConcurrentQueue<HttpResponseMessage> _queue;
        private readonly ILogger<HueProcessor> _logger;
        private readonly ApiConsumer _consumer;
        private readonly IPhilipsHueClient _philipsHueClient;

        public HueProcessor(ILogger<HueProcessor> logger, 
                            ApiConsumer consumer,
                            IPhilipsHueClient philipsHueClient)
        {
            _logger = logger;
            _consumer = consumer;
            _philipsHueClient = philipsHueClient;

            _queue = new();
        }

        public async Task KeepListeningAsync(CancellationToken cancellationToken = default)
        {
            var loop = InfinityUtil.BeyondAsync(25,
                                                TimeSpan.FromSeconds(2),
                                                TimeSpan.FromSeconds(10),
                                                cancellationToken);
            await foreach (var _ in loop)
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
                        var events = data.Where(d => d.Type == "update")
                                         .SelectMany(d => d.Data)
                                         .ToList();
                        await HandleEventsAsync(events);
                    }
                    else await Task.Delay(1000, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed single enqueue");
                }
            }
        }

        async Task HandleEventsAsync(IEnumerable<HueEventData> events)
        {
            foreach (var e in events)
            {
                var model = new DeviceChangedNotify
                {
                    Id = e.Id,
                };
                switch (e.Type)
                {
                    case "light":
                        model.Type = DeviceChangedNotify.DeviceType.HueLight;
                        break;
                    case "motion":
                        model.Type = DeviceChangedNotify.DeviceType.HueMotion;
                        break;
                }

                if (model.Type != DeviceChangedNotify.DeviceType.None)
                    await _consumer.NotifyDeviceChangeAsync(model);
            }
        }

    }
}

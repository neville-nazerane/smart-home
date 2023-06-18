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
        private readonly ListenerQueue _listenerQueue;
        private readonly IPhilipsHueClient _philipsHueClient;

        public HueProcessor(ILogger<HueProcessor> logger, 
                            ApiConsumer consumer,
                            ListenerQueue listenerQueue,
                            IPhilipsHueClient philipsHueClient)
        {
            _logger = logger;
            _consumer = consumer;
            _listenerQueue = listenerQueue;
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
                        HandleEvents(events);
                    }
                    else await Task.Delay(1000, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed single enqueue");
                }
            }
        }

        void HandleEvents(IEnumerable<HueEventData> events)
        {
            foreach (var e in events)
            {
                var model = new ListenedDevice
                {
                    Id = e.Id,
                };
                switch (e.Type)
                {
                    case "light":
                        model.DeviceType = DeviceType.HueLight;
                        break;
                    case "motion":
                        model.DeviceType = DeviceType.HueMotion;
                        break;
                }

                if (model.DeviceType != DeviceType.None)
                    _listenerQueue.Enqueue(model);

                //if (model.Type != DeviceType.None)
                //    await _consumer.NotifyDeviceChangeAsync(model);
            }
        }

    }
}

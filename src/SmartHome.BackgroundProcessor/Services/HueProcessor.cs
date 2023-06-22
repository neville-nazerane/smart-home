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
        private readonly BackgroundQueue<HttpResponseMessage> _queue;
        private readonly ILogger<HueProcessor> _logger;
        private readonly SignalRPusher _consumer;
        private readonly MainProcessor _mainProcessor;
        private readonly IPhilipsHueClient _philipsHueClient;

        public HueProcessor(ILogger<HueProcessor> logger, 
                            SignalRPusher consumer,
                            MainProcessor mainProcessor,
                            IPhilipsHueClient philipsHueClient)
        {
            _logger = logger;
            _consumer = consumer;
            _mainProcessor = mainProcessor;
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
            await foreach (var response in _queue.KeepDequeuingAsync(1000, cancellationToken))
            {
                try
                {
                    var data = await response.Content.ReadFromJsonAsync<IEnumerable<HueEvent>>(cancellationToken: cancellationToken);
                    response.Dispose();
                    var events = data.Where(d => d.Type == "update")
                                     .SelectMany(d => d.Data)
                                     .ToList();
                    await HandleEventsAsync(events, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed single enqueue");
                }
            }
        }

        Task HandleEventsAsync(IEnumerable<HueEventData> events, CancellationToken cancellationToken = default)
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
                    case "button":
                        if (e.Button.LastEvent == "initial_press")
                            model.DeviceType = DeviceType.HueButton;
                        break;
                }

                if (model.DeviceType != DeviceType.None)
                    return _mainProcessor.ListenAsync(model, cancellationToken);
            }
            return Task.CompletedTask;

        }

    }
}

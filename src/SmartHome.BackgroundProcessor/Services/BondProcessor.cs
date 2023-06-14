using SmartHome.ServerServices.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Threading;
using System.Text.Json;
using SmartHome.BackgroundProcessor.Exceptions;
using System.Collections.Concurrent;
using SmartHome.Models;
using SmartHome.BackgroundProcessor.Util;

namespace SmartHome.BackgroundProcessor.Services
{
    internal class BondProcessor
    {
        private const int PORT = 30007;

        private readonly ConcurrentQueue<UdpData> _queue;
        private readonly ILogger<BondProcessor> _logger;
        private readonly IBondClient _bondClient;
        private readonly ApiConsumer _apiConsumer;

        public BondProcessor(ILogger<BondProcessor> logger, IBondClient bondClient, ApiConsumer apiConsumer)
        {
            _logger = logger;
            _bondClient = bondClient;
            _apiConsumer = apiConsumer;
            _queue = new();
        }

        public async Task KeepListeningAsync(CancellationToken cancellationToken = default)
        {
            var loop = InfinityUtil.BeyondAsync(5, 
                                                TimeSpan.FromSeconds(2),
                                                TimeSpan.FromSeconds(10),
                                                cancellationToken);
            await foreach (int _ in loop)
            {
                try
                {
                    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                    cts.CancelAfter(TimeSpan.FromMinutes(5));

                    await RunUDPAsync(cts.Token);
                }
                catch (Exception ex)
                {
                    if (ex is not TaskCanceledException && ex is not OperationCanceledException)
                        _logger.LogError(ex, "Failed bond UDP processing");
                }
            }
        }

        async Task RunUDPAsync(CancellationToken cancellationToken = default)
        {
            using var client = new UdpClient();
            var ipEndPoint = new IPEndPoint(IPAddress.Parse(_bondClient.GetIp()), PORT);

            try
            {
                client.Connect(ipEndPoint); // connect to the provided IP address

                // send the Keep-Alive datagram to start the connection
                var keepAliveMessage = Encoding.UTF8.GetBytes("\n");
                await client.SendAsync(keepAliveMessage, keepAliveMessage.Length);

                while (true)
                {
                    var res = await client.ReceiveAsync(cancellationToken);
                    byte[] data = res.Buffer;
                    var model = JsonSerializer.Deserialize<UdpData>(data);
                    if (model.ErrorMessage is not null)
                        throw new BondUDPException(model.ErrorMessage);
                    _queue.Enqueue(model);
                }
            }
            finally
            {
                client.Close();
            }
        }

        public async Task ProcessQueueAsync(CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    if (_queue.TryDequeue(out var data) && data.UrlPath is not null)
                    {
                        var parts = data.UrlPath.Split("/");

                        if (parts.FirstOrDefault() == "devices" && parts.ElementAtOrDefault(2) == "actions")
                        {
                            string id = parts[1];
                            await HandleDeviceTriggeredByIdAsync(id);
                        }

                    }
                    else await Task.Delay(1000, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed single enqueue");
                }
            }
        }

        async Task HandleDeviceTriggeredByIdAsync(string id)
        {
            var model = new DeviceChangedNotify
            {
                Id = id,
                Type = DeviceChangedNotify.DeviceType.BondDevice
            };
            await _apiConsumer.NotifyDeviceChangeAsync(model);
        }

        class UdpData
        {
            [JsonPropertyName("err_msg")]
            public string ErrorMessage { get; set; }

            [JsonPropertyName("t")]
            public string UrlPath { get; set; }

        }

    }
}

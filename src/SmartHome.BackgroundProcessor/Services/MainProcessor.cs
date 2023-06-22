using SmartHome.Models;
using SmartHome.ServerServices.Automation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.BackgroundProcessor.Services
{
    public class MainProcessor
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<MainProcessor> _logger;
        private readonly SignalRPusher _apiConsumer;

        public MainProcessor(IServiceProvider serviceProvider, ILogger<MainProcessor> logger, SignalRPusher apiConsumer)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _apiConsumer = apiConsumer;
        }

        public Task ListenAsync(ListenedDevice device, CancellationToken cancellationToken = default)
        {
            return Task.WhenAll(
                PushToSignalRAsync(device, cancellationToken),
                TryRunAutomationAsync(device, cancellationToken)
            );
        }

        async Task TryRunAutomationAsync(ListenedDevice device, CancellationToken cancellationToken = default)
        {
            if (SmartDevices.GetListeningDeviceName(device) is not null)
            {
                await using var scope = _serviceProvider.CreateAsyncScope();
                var service = scope.ServiceProvider.GetService<AutomationService>();
                await service.OnDeviceListenedAsync(device, cancellationToken);
            }
        }

        Task PushToSignalRAsync(ListenedDevice device, CancellationToken cancellationToken)
        {
            try
            {
                return _apiConsumer.NotifyDeviceChangeAsync(device, cancellationToken);
            }
            catch (Exception ex)
            {
                if (cancellationToken.IsCancellationRequested) throw;
                _logger.LogError(ex, "Failed notifying device change (signalR)");
                return Task.CompletedTask;
            }
        }
    }
}

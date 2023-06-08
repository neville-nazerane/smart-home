using SmartHome.BackgroundProcessor.Services;
using SmartHome.ServerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.BackgroundProcessor
{
    public class HueListener : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public HueListener(IServiceProvider serviceProvider) 
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var processor = scope.ServiceProvider.GetService<HueProcessor>();

            var gpio = scope.ServiceProvider.GetService<IGpioService>();
            var consumer = scope.ServiceProvider.GetService<ApiConsumer>();

            await Task.WhenAll(
                    KeepLookingAsync(gpio, consumer),
                    processor.KeepListeningAsync(stoppingToken),
                    processor.ProcessQueueAsync(stoppingToken)
            );
        }

        async Task KeepLookingAsync(IGpioService gpio, ApiConsumer consumer)
        {
            while (true)
            {
                if (((GpioService)gpio).GetBinaryRead(4))
                {
                    await consumer.NotifyDeviceChangeAsync(new Models.DeviceChangedNotify
                    {
                        Id = "4",
                        Type = Models.DeviceChangedNotify.DeviceType.TouchSensor
                    });
                }
                await Task.Delay(500);
            }
        }

    }
}

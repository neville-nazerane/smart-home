using SmartHome.BackgroundProcessor.Services;
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
            await Task.WhenAll(
                    processor.KeepListeningAsync(stoppingToken),
                    processor.ProcessQueueAsync(stoppingToken));
        }

    }
}

using SmartHome.BackgroundProcessor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.BackgroundProcessor
{
    public class HueListener : Worker
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;

        public HueListener(ILogger<Worker> logger, IServiceProvider serviceProvider) : base(logger)
        {
            _logger = logger;
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

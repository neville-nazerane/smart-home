using SmartHome.BackgroundProcessor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.BackgroundProcessor
{
    public class MainListener : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public MainListener(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            await using var scope = _serviceProvider.CreateAsyncScope();
            var service = scope.ServiceProvider.GetService<MainProcessor>();
            await service.KeepRunningAsync(stoppingToken);
        }
    }
}

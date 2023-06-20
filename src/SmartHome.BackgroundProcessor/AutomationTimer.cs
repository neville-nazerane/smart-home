using SmartHome.ServerServices.Automation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.BackgroundProcessor
{
    public class AutomationTimer : BackgroundService
    {
        private readonly ILogger<AutomationTimer> _logger;
        private readonly IServiceProvider _serviceProvider;

        public AutomationTimer(ILogger<AutomationTimer> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
            => MinuiteTimerAsync(stoppingToken);

        async Task MinuiteTimerAsync(CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await using var scope = _serviceProvider.CreateAsyncScope();
                    var service = scope.ServiceProvider.GetService<AutomationService>();

                    await Task.WhenAll(
                        service.OnMinuiteTimerAsync(cancellationToken),
                        Task.Delay(TimeSpan.FromMinutes(1), cancellationToken)
                    );

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed Minuite timer");
                }
            }
        }

    }
}

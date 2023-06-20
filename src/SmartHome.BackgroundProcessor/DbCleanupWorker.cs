using Microsoft.EntityFrameworkCore;
using SmartHome.ServerServices;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.BackgroundProcessor
{
    public class DbCleanupWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DbCleanupWorker> _logger;

        public DbCleanupWorker(IServiceProvider serviceProvider, ILogger<DbCleanupWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }


        protected override Task ExecuteAsync(CancellationToken stoppingToken) => HourlyRunnerAsync(stoppingToken);

        async Task HourlyRunnerAsync(CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {

                try
                {

                    await using var scope = _serviceProvider.CreateAsyncScope();
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    var timeBreak = DateTime.UtcNow.AddHours(-1);

                    await db.DeviceLogs
                                   .Where(l => l.OccurredOn < timeBreak)
                                   .ExecuteDeleteAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed hourly db process");
                }
                await Task.Delay(TimeSpan.FromHours(1), cancellationToken);
            }
        }

    }
}

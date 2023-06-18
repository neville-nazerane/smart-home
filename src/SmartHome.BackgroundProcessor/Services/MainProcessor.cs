using SmartHome.Models;
using SmartHome.ServerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.BackgroundProcessor.Services
{
    public class MainProcessor
    {
        private readonly ListenerQueue _queue;
        private readonly AppDbContext _dbContext;
        private readonly ILogger<MainProcessor> _logger;
        private readonly ApiConsumer _apiConsumer;

        public MainProcessor(ListenerQueue queue, AppDbContext dbContext, ILogger<MainProcessor> logger, ApiConsumer apiConsumer)
        {
            _queue = queue;
            _dbContext = dbContext;
            _logger = logger;
            _apiConsumer = apiConsumer;
        }

        public async Task KeepRunningAsync(CancellationToken cancellationToken = default)
        {
            await foreach (var device in _queue.KeepDequeuingAsync(cancellationToken))
            {
                try
                {
                    string name = SmartDevices.GetListeningDeviceName(device.Id, device.DeviceType);
                    await _dbContext.DeviceLogs.AddAsync(new()
                    {
                        DeviceId = device.Id,
                        OccurredOn = DateTime.UtcNow,
                        DeviceType = device.DeviceType,
                        Name = name,
                    }, cancellationToken);
                    await _dbContext.SaveChangesAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    if (cancellationToken.IsCancellationRequested) throw;
                    _logger.LogError(ex, "Failed logging device");
                }
            }
        }


    }
}

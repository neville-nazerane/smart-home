using SmartHome.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.ServerServices
{
    public class AutomationService
    {
        private readonly AppDbContext _dbContext;

        public AutomationService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task DeviceListenedAsync(ListenedDevice device, CancellationToken cancellationToken)
        {
            string name = SmartDevices.GetListeningDeviceName(device.Id, device.DeviceType);
            await _dbContext.DeviceLogs.AddAsync(new()
            {
                DeviceId = device.Id,
                DeviceType = device.DeviceType,
                OccurredOn = DateTime.UtcNow,
                Name = name
            }, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}

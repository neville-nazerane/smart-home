using SmartHome.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.ServerServices.Automation
{
    public partial class AutomationService
    {
        private readonly AppDbContext _dbContext;
        private readonly SmartContext _smartContext;

        private SmartDevices Devices => _smartContext?.Devices;

        public AutomationService(AppDbContext dbContext, SmartContext smartContext)
        {
            _dbContext = dbContext;
            _smartContext = smartContext;
        }

        public async Task LogListenedAsync(ListenedDevice device, string name, string state)
        {
            await _dbContext.DeviceLogs.AddAsync(new()
            {
                DeviceId = device.Id,
                DeviceType = device.DeviceType,
                LoggedOn = DateTime.UtcNow,
                Name = name,
                State = state
            });
            await _dbContext.SaveChangesAsync();
        }

    }
}

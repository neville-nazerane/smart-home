﻿using SmartHome.Models;
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
        private IScenesService Scenes => _smartContext?.Scenes;

        public AutomationService(AppDbContext dbContext, SmartContext smartContext)
        {
            _dbContext = dbContext;
            _smartContext = smartContext;
        }

        public async Task LogListenedAsync(ListenedDevice device, string state)
        {
            await _dbContext.DeviceLogs.AddAsync(new()
            {
                DeviceId = device.Id,
                DeviceType = device.DeviceType,
                LoggedOn = DateTime.UtcNow,
                Name = SmartDevices.GetListeningDeviceName(device),
                State = state
            });
            await _dbContext.SaveChangesAsync();
        }

    }
}

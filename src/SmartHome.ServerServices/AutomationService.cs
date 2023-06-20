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
        public Task DeviceListenedAsync(ListenedDevice device, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}

using SmartHome.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SmartHome.ServerServices.Automation
{
    public partial class AutomationService
    {

        public async Task OnDeviceListenedAsync(ListenedDevice device, CancellationToken cancellationToken = default)
        {
            // closet sensor turn on closet
            await VerifyClosetMotionSensorAsync(device);
            await VerifyClosetButtonAsync(device);
            await VerifyBedroomControlAsync(device);
            await VerifyComputerButtonAsync(device);

        }

        public Task OnMinuiteTimerAsync(CancellationToken cancellationToken = default) 
            => ClosetMinuiteCheckAsync(cancellationToken);

    }
}

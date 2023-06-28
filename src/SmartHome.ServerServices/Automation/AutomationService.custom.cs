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
            // CLOSET
            await VerifyClosetMotionSensorAsync(device);
            await VerifyClosetButtonAsync(device);

            // BEDROOM
            await VerifyBedroomControlAsync(device);
            await VerifyComputerButtonAsync(device);
            await VerifyBedroomMotionsAsync(device);

            // FRONT
            await VerifyFrontDialAsync(device);
            await VerifyFrontControlAsync(device);
            await VerifyFrontMotionAsync(device);

        }

        public async Task OnMinuiteTimerAsync(CancellationToken cancellationToken = default)
        {
            await ClosetMinuteCheckAsync(cancellationToken);
            await BedroomMinuteCheckAsync(cancellationToken);
            await HueSyncMinuiteCheckAsync(cancellationToken);
        }
    }
}

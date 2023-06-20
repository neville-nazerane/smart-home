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



        async Task OnDeviceListenedAsync(ListenedDevice device, CancellationToken cancellationToken)
        {
            // closet sensor turn on closet
            await VerifyClosetMotionSensorAsync(device);

        }

        async ValueTask VerifyClosetMotionSensorAsync(ListenedDevice device)
        {
            if (Devices.ClosetMotionSensor.Is(device))
            {
                var motion = await Devices.ClosetMotionSensor.GetAsync();
                await LogListenedAsync(device, nameof(Devices.ClosetMotionSensor), motion.IsMotionDetected ? "motion" : "no-motion");

                if (motion.IsMotionDetected)
                    await _smartContext.Devices.ClosetLight.TriggerSwitchAsync(true);
            }
        }

    }
}

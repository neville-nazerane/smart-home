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

        }

        public Task OnMinuiteTimerAsync(CancellationToken cancellationToken = default) 
            => Task.WhenAll(ClosetMinuiteCheckAsync(cancellationToken));

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

        async Task ClosetMinuiteCheckAsync(CancellationToken cancellationToken = default)
        {
            var motion = await Devices.ClosetMotionSensor.GetAsync(cancellationToken);
            if (!motion.IsMotionDetected)
            {
                int lastNonMotionMins = (DateTime.UtcNow - motion.LastChanged.ToUniversalTime()).Minutes;
                if (lastNonMotionMins > 5)
                {
                    await Devices.ClosetLight.TriggerSwitchAsync(false, cancellationToken);
                }
            }
        }

    }
}

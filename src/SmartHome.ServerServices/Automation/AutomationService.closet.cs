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


        async ValueTask VerifyClosetMotionSensorAsync(ListenedDevice device)
        {
            if (Devices.ClosetMotionSensor.Is(device))
            {
                var motion = await Devices.ClosetMotionSensor.GetAsync();
                await LogListenedAsync(device, motion.IsMotionDetected ? "motion" : "no-motion");
                
                if (motion.IsMotionDetected)
                    await _smartContext.Devices.ClosetLight.TriggerSwitchAsync(true);
            }
        }

        async Task ClosetMinuteCheckAsync(CancellationToken cancellationToken = default)
        {
            var motion = await Devices.ClosetMotionSensor.GetAsync(cancellationToken);
            if (!motion.IsMotionDetected)
            {
                int lastNonMotionMins = (DateTime.UtcNow - motion.LastChanged.ToUniversalTime()).Minutes;
                if (lastNonMotionMins > 5)
                    await Devices.ClosetLight.TriggerSwitchAsync(false, cancellationToken);
            }
        }

        async ValueTask VerifyClosetButtonAsync(ListenedDevice device)
        {
            if (Devices.HueClosetButton.Is(device))
            {
                await LogListenedAsync(device, "pressed");
                var light = await Devices.ClosetLight.GetAsync();
                await Devices.ClosetLight.TriggerSwitchAsync(!light.IsSwitchedOn);
            }
        }

    }
}

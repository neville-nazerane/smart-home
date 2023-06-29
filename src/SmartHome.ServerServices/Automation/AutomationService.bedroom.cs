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

        async ValueTask VerifyComputerButtonAsync(ListenedDevice device)
        {
            if (device == Devices.ComputerButton)
            {
                await LogListenedAsync(device, "pressed");
                await Scenes.SwitchAsync(SceneName.Computer);
            }
        }

        async ValueTask VerifyBedroomControlAsync(ListenedDevice device)
        {
            if (device == Devices.BedroomControl)
            {
                string action = "pressed";
                var control = Devices.BedroomControl;
                if (device == control.IncreaseButton)
                    await Devices.BedroomCeilingFan.IncreaseAsync();
                else if (device == control.DecreaseButton)
                    await Devices.BedroomCeilingFan.DecreaseAsync();
                else if (device == control.OnOffButton)
                    await Scenes.SwitchAsync(SceneName.Bedroom);
                else if (device == control.HueButton)
                {
                    var button = await control.HueButton.GetAsync();
                    if (button.LastEvent.Contains("long"))
                    {
                        action = "long pressed";
                        await Scenes.SwitchAsync(SceneName.GoodNight);
                    }
                    else if (button.LastEvent.Contains("short_release"))
                    {
                        await Scenes.SwitchAsync(SceneName.GoodNight);
                        //await Scenes.SwitchAsync(SceneName.Snooze);
                    }
                }

                await LogListenedAsync(device, action);
            }

        }

        async ValueTask VerifyBedroomMotionsAsync(ListenedDevice device)
        {
            bool isSleeping = await Scenes.IsAnySceneEnabledAsync(SceneName.GoodNight, SceneName.Snooze);

            if (device == Devices.BedroomMotionSensor1 || device == Devices.BedroomMotionSensor2)
            {
                await LogListenedAsync(device, "motion");
                if (!isSleeping)
                    await Scenes.SetSceneEnabledAsync(SceneName.Bedroom, true);
            }

        }

        async Task BedroomMinuteCheckAsync(CancellationToken cancellationToken = default)
        {
            bool isSleeping = await Scenes.IsAnySceneEnabledAsync(SceneName.GoodNight, SceneName.Snooze);

            if (!isSleeping)
            {
                var motion1 = await Devices.BedroomMotionSensor1.GetAsync(cancellationToken);
                var motion2 = await Devices.BedroomMotionSensor2.GetAsync(cancellationToken);

                if (!motion1.IsMotionDetected && !motion2.IsMotionDetected)
                {
                    int lastNonMotionMins1 = (DateTime.UtcNow - motion1.LastChanged.ToUniversalTime()).Minutes;
                    int lastNonMotionMins2 = (DateTime.UtcNow - motion2.LastChanged.ToUniversalTime()).Minutes;

                    if (lastNonMotionMins1 > 5 && lastNonMotionMins2 > 5)
                        await Scenes.SetSceneEnabledAsync(SceneName.Bedroom, false, cancellationToken);
                }
            }

        }

    }
}

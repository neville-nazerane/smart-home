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
                    await Scenes.SetSceneEnabledAsync(SceneName.GoodNight, true);
                }
                else
                {
                    await Scenes.SetSceneEnabledAsync(SceneName.Snooze, true);
                }
            }
            else return;

            await LogListenedAsync(device, action);
        }

        async ValueTask VerifyBedroomMotionsAsync(ListenedDevice device)
        {

            if (device == Devices.BedroomMotionSensor1 || device == Devices.BedroomMotionSensor2)
            {
                await LogListenedAsync(device, "motion");
                await Scenes.SetSceneEnabledAsync(SceneName.Bedroom, true);
            }

        }

    }
}

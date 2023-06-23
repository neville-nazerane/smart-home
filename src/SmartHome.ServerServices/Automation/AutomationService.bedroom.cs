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

        async ValueTask VerifyBedroomControlAsync(ListenedDevice device)
        {
            string action = "press";
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
                    action = "long";
                    await Scenes.SetSceneEnabledAsync(SceneName.GoodNight, true);
                }
                else
                {
                    await Scenes.SetSceneEnabledAsync(SceneName.Snooze, true);
                }
            }

            await LogListenedAsync(device, action);
        }

    }
}

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

        async ValueTask VerifyFrontDialAsync(ListenedDevice device)
        {

            if (device == Devices.FrontDial)
            {
                await LogListenedAsync(device, "pressed");

                if (device == Devices.FrontDial.One)
                    await Scenes.SwitchAsync(SceneName.Kitchen);
                else if (device == Devices.FrontDial.Two)
                    await Scenes.SwitchAsync(SceneName.FrontRoom);

            }

        }

        async ValueTask VerifyFrontControlAsync(ListenedDevice device)
        {
            if (device == Devices.FrontControl)
            {
                await LogListenedAsync(device, "pressed");

                if (device == Devices.FrontControl.OnOffButton)
                    await Scenes.SwitchAsync(SceneName.FrontRoom);
                else if (device == Devices.FrontControl.IncreaseButton)
                    await Devices.FrontCeilingFan.IncreaseAsync();
                else if (device == Devices.FrontControl.DecreaseButton)
                    await Devices.FrontCeilingFan.DecreaseAsync();

            }

        }

    }
}

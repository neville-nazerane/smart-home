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

                string action = "pressed";

                if (device == Devices.FrontDial.One)
                    await Scenes.SwitchAsync(SceneName.Kitchen);
                else if (device == Devices.FrontDial.Two)
                {
                    var light = await Devices.MiddleLight.GetAsync();
                    await Devices.MiddleLight.TriggerSwitchAsync(!light.IsSwitchedOn);
                }
                else if (device == Devices.FrontDial.Three)
                {
                    await Devices.KitchenRoller.ToggleAsync();
                }
                else if (device == Devices.FrontDial.Rotary)
                {
                    action = "spun";
                    var dial = await Devices.FrontDial.Rotary.GetAsync();
                    var light = await Devices.MiddleLight.GetAsync();
                    double percent;
                    if (dial.IsLastRotatedClockWise)
                    {
                        if (light.Brightness == 100) return;
                        percent = Math.Min(100, light.Brightness + 10);
                    }
                    else
                    {
                        if (light.Brightness == 0) return;
                        percent = Math.Max(0, light.Brightness - 10);
                    }

                    await Devices.MiddleLight.SetBrightnessAsync(percent);
                }
                await LogListenedAsync(device, action);
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

        async ValueTask VerifyFrontMotionAsync(ListenedDevice device)
        {
            if (device == Devices.FrontMotionSensor)
            {
                await LogListenedAsync(device, "motion");
                await Scenes.SetSceneEnabledAsync(SceneName.FrontRoom, true);
            }
        }

    }
}

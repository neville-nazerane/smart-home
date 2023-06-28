using Microsoft.EntityFrameworkCore.Infrastructure;
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
                    await Devices.KitchenRoller.ToggleAsync();

                else if (device == Devices.FrontDial.Two)
                    await Scenes.SwitchAsync(SceneName.Kitchen);


                else if (device == Devices.FrontDial.Three)
                {
                    var light = await Devices.MiddleLight.GetAsync();
                    await Devices.MiddleLight.TriggerSwitchAsync(!light.IsSwitchedOn);
                }

                else if (device == Devices.FrontDial.Four)
                    await Scenes.SetSceneEnabledAsync(SceneName.Bedroom, false);

                else if (device == Devices.FrontDial.Rotary)
                {
                    action = "spun";
                    var dial = await Devices.FrontDial.Rotary.GetAsync();
                    var light = await Devices.MiddleLight.GetAsync();
                    double percent;
                    int increment = dial.Steps * 5;
                    if (dial.IsLastRotatedClockWise)
                    {
                        if (light.Brightness == 100) return;
                        percent = Math.Min(100, light.Brightness + increment);
                    }
                    else
                    {
                        if (light.Brightness == 0) return;
                        percent = Math.Max(0, light.Brightness - increment);
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
                var motion = await Devices.FrontMotionSensor.GetAsync();
                
                if (motion.IsMotionDetected)
                {
                    await LogListenedAsync(device, "motion");

                    if (await Scenes.IsEnabledAsync(SceneName.GoodNight))
                    {
                        await Devices.MiddleLight.TriggerSwitchAsync(true);
                    }
                    else
                    {
                        await Scenes.SetSceneEnabledAsync(SceneName.FrontRoom, true);
                    }
                }
                else await LogListenedAsync(device, "no motion");
            }
        }

        async ValueTask VerifyInsectPowerAsync(ListenedDevice device)
        {
            if (device == Devices.InsectPower)
            {
                var power = await Devices.InsectPower.GetAsync();
                if (power.IsSwitchedOn)
                    await Devices.InsectSwitch.TriggerSwitchAsync(true);
            }
        }

        async Task HueSyncMinuiteCheckAsync(CancellationToken cancellationToken = default)
        {
            var sceneIsEnabled = await Scenes.IsEnabledAsync(SceneName.TvSync, cancellationToken);
            if (sceneIsEnabled)
            {
                var isSyncing = await Devices.HueSync.GetSyncStateAsync(cancellationToken);
                if (!isSyncing)
                    await Devices.HueSync.SetSyncStateAsync(true, cancellationToken);
            }

            // force TV light to go off to account for HUE bug
            if (!await Scenes.IsEnabledAsync(SceneName.TvLights, cancellationToken))
            {
                var light = await Devices.TvLight.GetAsync(cancellationToken);
                if (light.IsSwitchedOn)
                {
                    await Devices.TvLight.TriggerSwitchAsync(false, cancellationToken);
                    await Devices.TvBottomLightStrip.TriggerSwitchAsync(false, cancellationToken);
                    await Devices.TvLeftBar.TriggerSwitchAsync(false, cancellationToken);
                    await Devices.TvRightBar.TriggerSwitchAsync(false, cancellationToken);
                }
            }
        }

        async Task MiddleLightMinuiteCheckAsync(CancellationToken cancellationToken = default)
        {
            bool isSceneEnabled = await Scenes.IsAnySceneEnabledAsync(SceneName.GoodNight);
            if (isSceneEnabled)
            {
                var motion = await Devices.FrontMotionSensor.GetAsync(cancellationToken);
                if (!motion.IsMotionDetected && (DateTime.UtcNow - motion.LastChanged.ToUniversalTime()).TotalMinutes > 5)
                {
                    await Devices.MiddleLight.TriggerSwitchAsync(false, cancellationToken);
                }

            }
            
        }

    }
}

﻿using Microsoft.Extensions.Logging;
using SmartHome.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.ServerServices.Scenes
{
    public partial class ScenesService
    {

        async Task KitchenTriggerAsync(bool state)
        {
            await Devices.KitchenLight1.TriggerSwitchAsync(state);
            await Devices.KitchenLight2.TriggerSwitchAsync(state);
        }

        async Task FrontRoomTriggerAsync(bool state)
        {
            await Scenes.SetSceneEnabledAsync(SceneName.FrontGoodNight, false);
            await Devices.MiddleLight.TriggerSwitchAsync(state);
            if (state)
                await Devices.MiddleLight.SetBrightnessAsync(100);
            try
            {
                await SetSceneEnabledAsync(SceneName.Kitchen, state);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to switch kitchen");
            }
            await SetSceneEnabledAsync(SceneName.TvLights, state);

            try
            {
                if (!state)
                    await Devices.FrontCeilingFan.SwitchAsync(state);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to switch fan");
            }

            try
            {
                await Devices.FrontCeilingFan.SwitchLightAsync(state);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to switch fan light");
            }

        }

        async Task FrontGoodNightTriggerAsync(bool state)
        {
            // DIM the middle light
            if (state)
            {
                await Devices.MiddleLight.TriggerSwitchAsync(true);
                await Devices.MiddleLight.SetBrightnessAsync(20);
                await Devices.MiddleLight.TriggerSwitchAsync(false);
            }

            await Devices.MiddleLight.TriggerSwitchAsync(!state);
            if (!state)
                await Devices.MiddleLight.SetBrightnessAsync(100);
            try
            {
                await SetSceneEnabledAsync(SceneName.Kitchen, !state);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to switch kitchen");
            }

            try
            {
                if (state)
                    await Devices.FrontCeilingFan.SwitchLightAsync(!state);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to switch fan light");
            }

            await SetSceneEnabledAsync(SceneName.TvLights, !state);
        }

        async Task TvLightsTriggerAsync(bool state)
        {
            if (!state && await Devices.HueSync.GetSyncStateAsync())
            {
                await SetSceneEnabledAsync(SceneName.TvSync, false);
                await Task.Delay(3000);
            }

            await Devices.TvBottomLightStrip.TriggerSwitchAsync(state);
            await Devices.TvLeftBar.TriggerSwitchAsync(state);
            await Devices.TvRightBar.TriggerSwitchAsync(state);
            await Devices.TvLight.TriggerSwitchAsync(state);
            if (state)
                await SetSceneEnabledAsync(SceneName.TvSync, true);
        }

        async Task MovieTriggerAsync(bool state)
        {
            // DIM/BRIGHT the middle light
            if (state)
            {
                await Devices.MiddleLight.TriggerSwitchAsync(true);
                await Devices.MiddleLight.SetBrightnessAsync(20);
                await Devices.MiddleLight.TriggerSwitchAsync(false);
            }
            else
            {
                await Devices.MiddleLight.TriggerSwitchAsync(true);
                await Devices.MiddleLight.SetBrightnessAsync(100);
            }

            await Task.WhenAll(
                Task.Run(async () =>
                {

                    try
                    {
                        await SetSceneEnabledAsync(SceneName.Kitchen, !state);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "Failed to switch kitchen");
                    }

                }),
                Task.Run(async () =>
                {

                    try
                    {
                        if (state)
                            await Devices.FrontCeilingFan.SwitchLightAsync(!state);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "Failed to switch fan light");
                    }

                })
            );

            await Devices.InsectPower.TriggerSwitchAsync(!state);
        }

        Task SyncTriggeredAsync(bool state) => Devices.HueSync.SetSyncStateAsync(state);

    }
}

using Microsoft.Extensions.Logging;
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
            await Devices.MiddleLight.TriggerSwitchAsync(state);
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
                _logger.LogError(e, "Failed to switch fan");
            }

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

        Task SyncTriggeredAsync(bool state) => Devices.HueSync.SetSyncStateAsync(state);

    }
}

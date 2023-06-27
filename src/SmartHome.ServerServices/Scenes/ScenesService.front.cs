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

            await Devices.FrontCeilingFan.SwitchAsync(state);
            await Devices.FrontCeilingFan.SwitchLightAsync(state);
        }

        async Task TvLightsTriggerAsync(bool state)
        {
            await Devices.TvBottomLightStrip.TriggerSwitchAsync(state);
            await Devices.TvLeftBar.TriggerSwitchAsync(state);
            await Devices.TvRightBar.TriggerSwitchAsync(state);
            await Devices.TvLight.TriggerSwitchAsync(state);
        }

    }
}

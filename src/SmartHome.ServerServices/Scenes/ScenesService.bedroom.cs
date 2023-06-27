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

        async Task BedroomTriggeredAsync(bool state)
        {
            await SetSceneEnabledAsync(SceneName.GoodNight, false);
            await SetSceneEnabledAsync(SceneName.Snooze, false);
            await SetSceneEnabledAsync(SceneName.Computer, state);

            await Devices.BedroomCeilingFan.SwitchLightAsync(state);
            await Devices.BedroomCeilingFan.SwitchAsync(state);

            if (!state)
                await Devices.ClosetLight.TriggerSwitchAsync(false);
        }

        async Task ComputerTriggeredAsync(bool state)
        {
            await Devices.ComputerLightPlug.TriggerSwitchAsync(state);

            await Devices.ComputerRightIris.TriggerSwitchAsync(state);
            await Devices.ComputerLeftIris.TriggerSwitchAsync(state);

            await Devices.ComputerRightBar.TriggerSwitchAsync(state);
            await Devices.ComputerLeftBar.TriggerSwitchAsync(state);
        }

        async Task GoodNightTriggeredAsync(bool state)
        {
            await SetSceneEnabledAsync(SceneName.Computer, !state);
            try
            {
                await Devices.BedroomCeilingFan.SwitchLightAsync(!state);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to switch bedroom light");
            }
            await SetSceneEnabledAsync(SceneName.FrontRoom, !state);
        }

    }
}

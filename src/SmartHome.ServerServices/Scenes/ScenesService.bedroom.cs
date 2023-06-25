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

            if (state)
            {
                await Devices.BedroomCeilingFan.TurnLightOnAsync();
                await Devices.BedroomCeilingFan.TurnOnAsync();
            }
            else
            {
                await Devices.ClosetLight.TriggerSwitchAsync(false);
                await Devices.BedroomCeilingFan.TurnLightOffAsync();
                await Devices.BedroomCeilingFan.TurnOffAsync();
            }
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
            if (state)
            {
                await Devices.BedroomCeilingFan.TurnLightOffAsync();
            }
            else
            {
                await Devices.BedroomCeilingFan.TurnLightOnAsync();
            }
            await SetSceneEnabledAsync(SceneName.FrontRoom, !state);
        }

    }
}

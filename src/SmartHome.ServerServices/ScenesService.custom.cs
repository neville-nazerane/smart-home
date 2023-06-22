using SmartHome.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.ServerServices
{
    public partial class ScenesService
    {

        async Task OnSceneChangedAsync(SceneName name)
        {
            bool state = await IsEnabledAsync(SceneName.Bedroom);
            switch (name)
            {
                case SceneName.GoodNight:
                    break;
                case SceneName.Snooze:
                    break;
                case SceneName.Computer:
                    await ComputerTriggeredAsync(state);
                    break;
                case SceneName.Bedroom:
                    await BedroomTriggeredAsync(state);
                    break;
                case SceneName.FrontRoom:
                    break;
            }
        }

        async Task BedroomTriggeredAsync(bool state)
        {
            await SetSceneEnabledAsync(SceneName.GoodNight, false);
            await SetSceneEnabledAsync(SceneName.Snooze, false);
            await SetSceneEnabledAsync(SceneName.Computer, state);

            if (state)
            {
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

    }
}

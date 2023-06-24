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
            await SetSceneEnabledAsync(SceneName.Kitchen, state);

            if (state)
            {
                await Devices.FrontCeilingFan.TurnOnAsync();
                await Devices.FrontCeilingFan.TurnLightOnAsync();
            }
            else
            {
                await Devices.FrontCeilingFan.TurnOffAsync();
                await Devices.FrontCeilingFan.TurnLightOffAsync();
            }

        }

    }
}

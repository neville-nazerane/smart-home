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

        async Task OnSceneChangedAsync(SceneName name)
        {
            bool state = await IsEnabledAsync(name);
            switch (name)
            {
                case SceneName.GoodNight:
                    await GoodNightTriggeredAsync(state);
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
                    await FrontRoomTriggerAsync(state);
                    break;
                case SceneName.Kitchen:
                    await KitchenTriggerAsync(state);
                    break;
            }
        }



    }
}

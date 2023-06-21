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

        async ValueTask VerifyBedroomControlAsync(ListenedDevice device)
        {
            var control = Devices.BedroomControl;
            if (device == control.IncreaseButton)
                await Devices.BedroomCeilingFan.IncreaseAsync();
            else if (device == control.DecreaseButton)
                await Devices.BedroomCeilingFan.DecreaseAsync();

        }

    }
}

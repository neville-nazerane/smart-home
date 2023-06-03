using SmartHome.Models.PhilipsHue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SmartHome.Models.SmartContextBase;

namespace SmartHome.Models.ClientContracts
{
    public interface IPhilipsHueClient
    {
        Task SwitchLightAsync(LightRequestModel request, bool switchOn, CancellationToken cancellationToken = default);
    }
}

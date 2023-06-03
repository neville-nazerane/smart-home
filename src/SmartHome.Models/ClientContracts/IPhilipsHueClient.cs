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
        Task<IEnumerable<LightModel>> GetAllLightsAsync(CancellationToken cancellationToken = default);
        Task SwitchLightAsync(HueLightRequestModel request, bool switchOn, CancellationToken cancellationToken = default);
    }
}

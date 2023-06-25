using SmartHome.Models.PhilipsHue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SmartHome.Models.SmartContextBase;

namespace SmartHome.Models.Contracts
{
    public interface IPhilipsHueClient
    {
        Task<IEnumerable<LightModel>> GetAllLightsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<MotionModel>> GetAllMotionSensorsAsync(CancellationToken cancellationToken = default);
        Task<ButtonModel> GetButtonAsync(string id, CancellationToken cancellationToken = default);
        Task<LightModel> GetLightAsync(string id, CancellationToken cancellationToken = default);
        Task<MotionModel> GetMotionSensorAsync(string id, CancellationToken cancellationToken = default);
        Task SetBrightnessAsync(string id, double percent, CancellationToken cancellationToken = default);
        Task SetLightColorAsync(string id, string colorHex, CancellationToken cancellationToken = default);
        Task SwitchLightAsync(string id, bool switchOn, CancellationToken cancellationToken = default);
    }
}

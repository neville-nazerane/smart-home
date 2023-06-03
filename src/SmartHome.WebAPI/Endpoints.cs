using SmartHome.Models.ClientContracts;
using SmartHome.ServerServices;
using HueModels = SmartHome.Models.PhilipsHue;
using static SmartHome.Models.SmartContextBase;

namespace SmartHome.WebAPI
{
    public static class Endpoints
    {

        public static void MapAllEndpoints(this WebApplication app)
        {

            app.MapGet("/philipsHue/lights", GetAllHueLightsAsync);
            app.MapPut("/philipsHue/switchLight/{switchOn}", HueLightSwitchAsync);

        }



        public static Task<IEnumerable<HueModels.LightModel>> GetAllHueLightsAsync(IPhilipsHueClient client, CancellationToken cancellationToken = default)
            => client.GetAllLightsAsync(cancellationToken);

        public static Task HueLightSwitchAsync(IPhilipsHueClient client,
                                               HueLightRequestModel model,
                                               bool switchOn,
                                               CancellationToken cancellationToken = default)
            => client.SwitchLightAsync(model, switchOn, cancellationToken);

    }
}

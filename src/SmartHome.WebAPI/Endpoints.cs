using SmartHome.Models.ClientContracts;
using SmartHome.ServerServices;
using HueModels = SmartHome.Models.PhilipsHue;
//using static SmartHome.Models.SmartContextBase;

namespace SmartHome.WebAPI
{
    public static class Endpoints
    {

        public static void MapAllEndpoints(this WebApplication app)
        {

            app.MapGet("/philipsHue/lights", GetAllHueLightsAsync);
            app.MapPut("/philipsHue/switchLight/{id}/{switchOn}", HueLightSwitchAsync);

        }



        public static Task<IEnumerable<HueModels.LightModel>> GetAllHueLightsAsync(IPhilipsHueClient client, CancellationToken cancellationToken = default)
            => client.GetAllLightsAsync(cancellationToken);

        public static Task HueLightSwitchAsync(SmartContext context,
                                               bool switchOn,
                                               string id,
                                               CancellationToken cancellationToken = default)
            => context.MakeHueLightRequestModel(id).TriggerSwitchAsync(switchOn, cancellationToken);
            //=> client.SwitchLightAsync(model, switchOn, cancellationToken);

    }
}

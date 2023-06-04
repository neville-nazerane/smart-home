using Microsoft.AspNetCore.SignalR;
using SmartHome.Models;
using SmartHome.Models.ClientContracts;
using SmartHome.ServerServices;
using SmartHome.WebAPI.Hubs;
using HueModels = SmartHome.Models.PhilipsHue;
//using static SmartHome.Models.SmartContextBase;

namespace SmartHome.WebAPI
{
    public static class Endpoints
    {

        public static void MapAllEndpoints(this WebApplication app)
        {
            app.MapPost("/notifyDeviceChange", NotifyDeviceChangeAsync);

            app.MapGet("/philipsHue/lights", GetAllHueLightsAsync);
            app.MapGet("/philipsHue/light/{id}", HueGetLightAsync);
            app.MapPut("/philipsHue/switchLight/{id}/{switchOn}", HueLightSwitchAsync);

        }

        public static Task NotifyDeviceChangeAsync(IHubContext<ChangeNotifyHub> hubContext,
                                                   DeviceChangedNotify model,
                                                   CancellationToken cancellationToken = default)
            => hubContext.Clients.All.SendAsync("deviceChanged", model, cancellationToken);

        #region Philips Hue

        public static Task<IEnumerable<HueModels.LightModel>> GetAllHueLightsAsync(IPhilipsHueClient client, CancellationToken cancellationToken = default)
            => client.GetAllLightsAsync(cancellationToken);

        public static Task HueLightSwitchAsync(SmartContext context,
                                               bool switchOn,
                                               string id,
                                               CancellationToken cancellationToken = default)
            => context.MakeHueLightRequestModel(id).TriggerSwitchAsync(switchOn, cancellationToken);

        public static Task<HueModels.LightModel> HueGetLightAsync(SmartContext context, 
                                            string id,
                                            CancellationToken cancellationToken = default)
            => context.MakeHueLightRequestModel(id).GetAsync(cancellationToken);

        #endregion

    }
}

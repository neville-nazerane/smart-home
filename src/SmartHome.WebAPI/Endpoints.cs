using Microsoft.AspNetCore.SignalR;
using SmartHome.Models;
using SmartHome.Models.Bond;
using SmartHome.Models.ClientContracts;
using SmartHome.ServerServices;
using SmartHome.ServerServices.Clients;
using SmartHome.WebAPI.Hubs;
using System.Reflection.Metadata;
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
            app.MapGet("/philipsHue/motions", GetAllHueMotionAsync);
            app.MapGet("/philipsHue/light/{id}", HueGetLightAsync);
            app.MapGet("/philipsHue/motion/{id}", HueGetMotionAsync);
            app.MapPut("/philipsHue/switchLight/{id}/{switchOn}", HueLightSwitchAsync);

            app.MapGet("/bond/ceilingFans", GetCeilingFansAsync);
            app.MapGet("/bond/ceilingFan/{id}", GetCeilingFanAsync);
            app.MapGet("/bond/rollers", GetRollersAsync);
            app.MapGet("/bond/roller/{id}", GetRollerAsync);
        }

        static Task NotifyDeviceChangeAsync(IHubContext<ChangeNotifyHub> hubContext,
                                                   DeviceChangedNotify model,
                                                   CancellationToken cancellationToken = default)
            => hubContext.Clients.All.SendAsync("deviceChanged", model, cancellationToken);

        #region Philips Hue

        public static Task<IEnumerable<HueModels.LightModel>> GetAllHueLightsAsync(IPhilipsHueClient client, CancellationToken cancellationToken = default)
            => client.GetAllLightsAsync(cancellationToken);
        public static Task<IEnumerable<HueModels.MotionModel>> GetAllHueMotionAsync(IPhilipsHueClient client, CancellationToken cancellationToken = default)
            => client.GetAllMotionSensorsAsync(cancellationToken);

        public static Task HueLightSwitchAsync(IPhilipsHueClient client,
                                               bool switchOn,
                                               string id,
                                               CancellationToken cancellationToken = default)
            => client.SwitchLightAsync(id, switchOn, cancellationToken);

        public static Task<HueModels.LightModel> HueGetLightAsync(IPhilipsHueClient client, 
                                            string id,
                                            CancellationToken cancellationToken = default)
            => client.GetLightAsync(id, cancellationToken);

        public static Task<HueModels.MotionModel> HueGetMotionAsync(IPhilipsHueClient client,
                                                                    string id,
                                                                    CancellationToken cancellationToken = default)
            => client.GetMotionSensorAsync(id, cancellationToken);

        #endregion

        #region Bond

        static Task<CeilingFanModel> GetCeilingFanAsync(IBondClient bondClient, string id, CancellationToken cancellationToken = default)
            => bondClient.GetCeilingFanAsync(id, cancellationToken);

        static Task<IEnumerable<CeilingFanModel>> GetCeilingFansAsync(IBondClient bondClient,
                                                                      CancellationToken cancellationToken = default)
            => bondClient.GetCeilingFansAsync(cancellationToken);

        static Task<RollerModel> GetRollerAsync(IBondClient bondClient, string id, CancellationToken cancellationToken = default)
            => bondClient.GetRollerAsync(id, cancellationToken);

        static Task<IEnumerable<RollerModel>> GetRollersAsync(IBondClient bondClient,
                                                              CancellationToken cancellationToken = default)
            => bondClient.GetRollersAsync(cancellationToken); 

        #endregion

    }
}

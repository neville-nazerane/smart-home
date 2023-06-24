using Microsoft.AspNetCore.SignalR;
using SmartHome.Models;
using SmartHome.Models.Bond;
using SmartHome.Models.Contracts;
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
            app.MapPost("/notifySceneChange", NotifySceneChangeAsync);

            app.MapGet("/listeningLogs", GetListeningLogsAsync);

            app.MapGet("/scenes", GetScenesAsync);
            app.MapGet("/scene/{sceneName}", IsEnabledAsync);
            app.MapPost("/scene/anyEnabled", IsAnyEnabledAsync);
            app.MapPut("/scene/{sceneName}/switch", SwitchSceneAsync);
            app.MapPut("/scene/{sceneName}/{isEnabled}", SetSceneEnabled);

            app.MapGet("/philipsHue/lights", GetAllHueLightsAsync);
            app.MapGet("/philipsHue/motions", GetAllHueMotionAsync);
            app.MapGet("/philipsHue/light/{id}", HueGetLightAsync);
            app.MapGet("/philipsHue/motion/{id}", HueGetMotionAsync);
            app.MapGet("/philipsHue/button/{id}", HueGetButtonAsync);
            app.MapPut("/philipsHue/switchLight/{id}/{switchOn}", HueLightSwitchAsync);
            app.MapPut("/philipsHue/color/{id}/{colorHex}", HueLightColorAsync);

            app.MapGet("/bond/ceilingFans", GetCeilingFansAsync);
            app.MapGet("/bond/ceilingFan/{id}", GetCeilingFanAsync);
            app.MapPut("/bond/ceilingFan/{id}/increase", IncreaseFanAsync);
            app.MapPut("/bond/ceilingFan/{id}/decrease", DecreaseFanAsync);
            app.MapPut("/bond/ceilingFan/{id}/on", TurnOnFanAsync);
            app.MapPut("/bond/ceilingFan/{id}/off", TurnOffFanAsync);
            app.MapPut("/bond/ceilingFan/{id}/lightOn", TurnOnFanLightAsync);
            app.MapPut("/bond/ceilingFan/{id}/lightOff", TurnOffFanLightAsync);
            app.MapGet("/bond/rollers", GetRollersAsync);
            app.MapGet("/bond/roller/{id}", GetRollerAsync);

            app.MapPost("/smartthings/switchbot/{deviceId}/trigger/{isOn}", TriggerSwitchBotAsync);
        }

        static Task NotifyDeviceChangeAsync(ISignalRPusher signalRPusher,
                                            ListenedDevice model,
                                            CancellationToken cancellationToken = default)
            => signalRPusher.NotifyDeviceChangeAsync(model, cancellationToken);

        static Task NotifySceneChangeAsync(ISignalRPusher signalRPusher,
                                           Scene scene,
                                           CancellationToken cancellationToken = default)
            => signalRPusher.NotifySceneChangeAsync(scene, cancellationToken);

        static Task<IEnumerable<DeviceLog>> GetListeningLogsAsync(SmartContext context,
                                                          int pageNumber,
                                                          int pageSize,
                                                          CancellationToken cancellationToken = default)
            => context.GetListeningLogsAsync(pageNumber, pageSize, cancellationToken);


        #region scenes

        static Task<IEnumerable<Scene>> GetScenesAsync(IScenesService service, CancellationToken cancellationToken = default)
            => service.GetAllScenesAsync(cancellationToken);

        static Task SwitchSceneAsync(IScenesService service, SceneName sceneName, CancellationToken cancellationToken = default)
            => service.SwitchAsync(sceneName, cancellationToken);

        static Task<bool> IsEnabledAsync(IScenesService service,
                                         SceneName sceneName,
                                         CancellationToken cancellationToken = default)
            => service.IsEnabledAsync(sceneName, cancellationToken);

        static Task SetSceneEnabled(IScenesService service,
                                    SceneName sceneName,
                                    bool isEnabled,
                                    CancellationToken cancellationToken = default)
            => service.SetSceneEnabledAsync(sceneName, isEnabled, cancellationToken);

        static Task<bool> IsAnyEnabledAsync(IScenesService service, SceneName[] sceneNames)
            => service.IsAnySceneEnabledAsync(sceneNames);

        #endregion

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

        public static Task HueLightColorAsync(IPhilipsHueClient client,
                                               string colorHex,
                                               string id,
                                               CancellationToken cancellationToken = default)
            => client.SetLightColorAsync(id, $"#{colorHex}", cancellationToken);

        public static Task<HueModels.LightModel> HueGetLightAsync(IPhilipsHueClient client,
                                                                    string id,
                                                                    CancellationToken cancellationToken = default)
            => client.GetLightAsync(id, cancellationToken);

        public static Task<HueModels.MotionModel> HueGetMotionAsync(IPhilipsHueClient client,
                                                                    string id,
                                                                    CancellationToken cancellationToken = default)
            => client.GetMotionSensorAsync(id, cancellationToken);


        public static Task<HueModels.ButtonModel> HueGetButtonAsync(IPhilipsHueClient client,
                                                                    string id,
                                                                    CancellationToken cancellationToken = default)
            => client.GetButtonAsync(id, cancellationToken);

        #endregion

        #region Bond


        static Task TurnOnFanAsync(IBondClient bondClient, string id, CancellationToken cancellationToken = default)
            => bondClient.TurnOnFanAsync(id, cancellationToken);

        static Task TurnOffFanAsync(IBondClient bondClient, string id, CancellationToken cancellationToken = default)
            => bondClient.TurnOffFanAsync(id, cancellationToken);

        static Task TurnOnFanLightAsync(IBondClient bondClient, string id, CancellationToken cancellationToken = default)
            => bondClient.TurnOnFanLightAsync(id, cancellationToken);

        static Task TurnOffFanLightAsync(IBondClient bondClient, string id, CancellationToken cancellationToken = default)
            => bondClient.TurnOffFanLightAsync(id, cancellationToken);


        static Task DecreaseFanAsync(IBondClient bondClient, string id, CancellationToken cancellationToken = default)
            => bondClient.DecreaseFanAsync(id, cancellationToken);

        static Task IncreaseFanAsync(IBondClient bondClient, string id, CancellationToken cancellationToken = default)
            => bondClient.IncreaseFanAsync(id, cancellationToken);

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

        #region switch bot

        static Task TriggerSwitchBotAsync(ISmartThingsClient client,
                                           string deviceId,
                                           bool isOn,
                                           CancellationToken cancellationToken = default)
            => client.TriggerSwitchBotAsync(deviceId, isOn, cancellationToken);

        #endregion

    }
}

using Microsoft.AspNetCore.SignalR;
using SmartHome.Models;
using SmartHome.ServerServices;
using SmartHome.WebAPI.Hubs;

namespace SmartHome.WebAPI.Services
{
    public class SignalRPusher : ISignalRPusher
    {
        private readonly IHubContext<ChangeNotifyHub> _hubContext;

        public SignalRPusher(IHubContext<ChangeNotifyHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public Task NotifyDeviceChangeAsync(ListenedDevice model,
                                            CancellationToken cancellationToken = default)
            => _hubContext.Clients.All.SendAsync("deviceChanged", model, cancellationToken);

        public Task NotifySceneChangeAsync(Scene scene,
                                          CancellationToken cancellationToken = default)
            => _hubContext.Clients.All.SendAsync("sceneChanged", scene, cancellationToken);

    }
}

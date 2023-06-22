using SmartHome.Models;

namespace SmartHome.ServerServices
{
    public interface ISignalRPusher
    {
        Task NotifyDeviceChangeAsync(ListenedDevice model, CancellationToken cancellationToken = default);
        Task NotifySceneChangeAsync(Scene scene, CancellationToken cancellationToken = default);
    }
}
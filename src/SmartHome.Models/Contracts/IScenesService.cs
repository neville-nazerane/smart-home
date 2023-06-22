using SmartHome.Models;

namespace SmartHome.ServerServices
{
    public interface IScenesService
    {
        Task<IEnumerable<Scene>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<bool> IsEnabledAsync(SceneName sceneName, CancellationToken cancellationToken = default);
        Task SetSceneEnabled(SceneName sceneName, bool isEnabled, CancellationToken cancellationToken = default);
    }
}
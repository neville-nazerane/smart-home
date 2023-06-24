using SmartHome.Models;

namespace SmartHome.ServerServices
{
    public interface IScenesService
    {
        Task<IEnumerable<Scene>> GetAllScenesAsync(CancellationToken cancellationToken = default);
        Task<bool> IsAnySceneEnabledAsync(params SceneName[] sceneNames);
        Task<bool> IsEnabledAsync(SceneName sceneName, CancellationToken cancellationToken = default);
        Task SetSceneEnabledAsync(SceneName sceneName, bool isEnabled, CancellationToken cancellationToken = default);
        Task SwitchAsync(SceneName sceneName, CancellationToken cancellationToken = default);
    }
}
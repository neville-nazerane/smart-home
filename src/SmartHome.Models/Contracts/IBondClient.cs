using SmartHome.Models.Bond;

namespace SmartHome.Models.Contracts
{
    public interface IBondClient
    {
        Task DecreaseFanAsync(string id, CancellationToken cancellationToken = default);
        Task<CeilingFanModel> GetCeilingFanAsync(string id, CancellationToken cancellationToken = default);
        Task<IEnumerable<CeilingFanModel>> GetCeilingFansAsync(CancellationToken cancellationToken = default);
        Task<RollerModel> GetRollerAsync(string id, CancellationToken cancellationToken = default);
        Task<IEnumerable<RollerModel>> GetRollersAsync(CancellationToken cancellationToken = default);
        Task IncreaseFanAsync(string id, CancellationToken cancellationToken = default);
        Task SwitchFanAsync(string id, bool isOn, CancellationToken cancellationToken = default);
        Task SwitchFanLightAsync(string id, bool isOn, CancellationToken cancellationToken = default);
        Task ToggleRollerAsync(string id, CancellationToken cancellationToken = default);

    }
}
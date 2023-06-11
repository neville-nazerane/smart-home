using SmartHome.Models.Bond;

namespace SmartHome.ServerServices.Clients
{
    public interface IBondClient
    {
        Task<IEnumerable<CeilingFanModel>> GetCeilingFansAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<RollerModel>> GetRollersAsync(CancellationToken cancellationToken = default);
    }
}
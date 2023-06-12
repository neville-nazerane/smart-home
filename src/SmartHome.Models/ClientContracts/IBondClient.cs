﻿using SmartHome.Models.Bond;

namespace SmartHome.ServerServices.Clients
{
    public interface IBondClient
    {
        Task DecreaseFanAsync(string id, CancellationToken cancellationToken = default);
        Task<CeilingFanModel> GetCeilingFanAsync(string id, CancellationToken cancellationToken = default);
        Task<IEnumerable<CeilingFanModel>> GetCeilingFansAsync(CancellationToken cancellationToken = default);
        Task<RollerModel> GetRollerAsync(string id, CancellationToken cancellationToken = default);
        Task<IEnumerable<RollerModel>> GetRollersAsync(CancellationToken cancellationToken = default);
        Task IncreaseFanAsync(string id, CancellationToken cancellationToken = default);
    }
}
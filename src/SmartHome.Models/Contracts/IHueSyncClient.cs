using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Models.Contracts
{
    public interface IHueSyncClient
    {
        Task<bool> GetSyncStateAsync(CancellationToken cancellationToken = default);
        Task SetSyncStateAsync(bool state, CancellationToken cancellationToken = default);
    }
}

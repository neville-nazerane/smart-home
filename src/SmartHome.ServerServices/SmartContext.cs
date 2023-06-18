using Microsoft.EntityFrameworkCore;
using SmartHome.Models;
using SmartHome.Models.ClientContracts;
using SmartHome.Models.PhilipsHue;
using SmartHome.ServerServices.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.ServerServices
{
    public class SmartContext : SmartContextBase
    {
        private readonly AppDbContext _dbContext;

        protected override IPhilipsHueClient PhilipsHueClient { get; }

        protected override IBondClient BondClient { get; }

        public SmartContext(IPhilipsHueClient philipsHueClient,
                            IBondClient bondClient,
                            AppDbContext dbContext) : base()
        {
            PhilipsHueClient = philipsHueClient;
            BondClient = bondClient;
            _dbContext = dbContext;
        }

        public override IAsyncEnumerable<DeviceLog> GetListeningLogsAsync(int pageNumber,
                                                                             int pageSize,
                                                                             CancellationToken cancellationToken = default)
            => _dbContext.DeviceLogs
                           .Skip((pageNumber - 1) * pageSize)
                           .Take(pageSize)
                           .OrderByDescending(l => l.OccurredOn)
                           .AsAsyncEnumerable();

    }
}

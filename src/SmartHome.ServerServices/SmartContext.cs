using Microsoft.EntityFrameworkCore;
using SmartHome.Models;
using SmartHome.Models.Contracts;
using SmartHome.Models.PhilipsHue;
using System;
using System.Collections;
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

        protected override ISmartThingsClient SmartThingsClient { get; }

        public override IScenesService Scenes { get; }
        public override IHueSyncClient HueSyncClient { get; }

        public SmartContext(IPhilipsHueClient philipsHueClient,
                            IBondClient bondClient,
                            IScenesService scenesService,
                            ISmartThingsClient smartThingsClient,
                            IHueSyncClient hueSyncClient,
                            AppDbContext dbContext) : base()
        {
            PhilipsHueClient = philipsHueClient;
            BondClient = bondClient;
            Scenes = scenesService;
            SmartThingsClient = smartThingsClient;
            HueSyncClient = hueSyncClient;
            _dbContext = dbContext;
        }


        public override async Task<IEnumerable<DeviceLog>> GetListeningLogsAsync(int pageNumber,
                                                                             int pageSize,
                                                                             CancellationToken cancellationToken = default)
            => await _dbContext.DeviceLogs
                                .OrderByDescending(l => l.LoggedOn)
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync(cancellationToken);



    }
}

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

        protected override IPhilipsHueClient PhilipsHueClient { get; }

        protected override IBondClient BondClient { get; }

        public SmartContext(IPhilipsHueClient philipsHueClient, IBondClient bondClient) : base()
        {
            PhilipsHueClient = philipsHueClient;
            BondClient = bondClient;
        }

    }
}

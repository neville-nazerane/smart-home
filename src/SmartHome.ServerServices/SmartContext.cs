using SmartHome.Models;
using SmartHome.Models.ClientContracts;
using SmartHome.Models.PhilipsHue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.ServerServices
{
    public class SmartContext : SmartContextBase
    {
        public SmartContext(IPhilipsHueClient philipsHueClient) : base(philipsHueClient)
        {
        }

    }
}

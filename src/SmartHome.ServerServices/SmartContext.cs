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

        private readonly IGpioService _gpioService;

        protected override IPhilipsHueClient PhilipsHueClient { get; }

        public SmartContext(IPhilipsHueClient philipsHueClient, IGpioService gpioService) : base()
        {
            PhilipsHueClient = philipsHueClient;
            _gpioService = gpioService;
        }



    }
}

using SmartHome.Models.ClientContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Models.PhilipsHue
{
    public class LightModel : DeviceModelBase
    {

        public bool IsSwitchedOn { get; set; }
        public string ColorHex { get; set; }

        public LightModel() : base(DeviceType.HueLight)
        {
        }

    }



}

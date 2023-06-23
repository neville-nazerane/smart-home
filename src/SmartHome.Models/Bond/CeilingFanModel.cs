using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Models.Bond
{
    public class CeilingFanModel : DeviceModelBase
    {

        public short Speed { get; set; }

        public bool LightIsOn { get; set; }

        public CeilingFanModel(): base(DeviceType.BondFan)
        {

        }
    }
}

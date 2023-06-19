using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Models.Bond
{
    public class RollerModel : DeviceModelBase
    {

        public bool IsOpen { get; set; }

        public RollerModel() : base(DeviceType.BondRoller)
        {
            
        }

    }
}

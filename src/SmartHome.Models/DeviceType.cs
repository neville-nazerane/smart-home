using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Models
{
    public enum DeviceType
    {
        None = 0,
        HueMotion = 1,
        HueLight = 2,
        BondFan = 3,
        BondRoller = 4,
        HueButton = 5,
        SwitchBot = 6,
        HueRotary = 7
    }
}

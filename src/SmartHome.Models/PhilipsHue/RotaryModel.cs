using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Models.PhilipsHue
{
    public class RotaryModel : DeviceModelBase
    {

        public bool IsLastRotatedClockWise { get; set; }

        public DateTime? LastUpdated { get; set; }
        public int Steps { get; set; }

        public RotaryModel() : base(DeviceType.HueRotary)
        {
        }
    }
}

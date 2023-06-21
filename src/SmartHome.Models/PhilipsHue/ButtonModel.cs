using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Models.PhilipsHue
{
    public class ButtonModel : DeviceModelBase
    {
        public ButtonModel() : base(DeviceType.HueButton)
        {
        }

        public string LastEvent { get; set; }

        public DateTime? LastEventExecutedOn { get; set; }

    }
}

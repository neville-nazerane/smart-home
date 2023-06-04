using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Models
{
    public class DeviceChangedNotify
    {

        public string Id { get; set; }

        public DeviceType Type { get; set; }

        public enum DeviceType
        {
            HueLight
        }

    }
}

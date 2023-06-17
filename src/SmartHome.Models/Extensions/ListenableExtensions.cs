using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Models.Extensions
{
    public static class ListenableExtensions
    {

        public static bool Is(this IListenableDevice device, ListenedDevice listened)
            => device.DeviceType == listened.DeviceType && device.Id == listened.Id;

    }
}

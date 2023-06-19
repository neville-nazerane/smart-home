using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Models
{
    public abstract class RequestableDeviceBase
    {

        public string Id { get; }

        public DeviceType DeviceType { get; }

        protected RequestableDeviceBase(string id, DeviceType deviceType)
        {
            Id = id;
            DeviceType = deviceType;
        }


        public bool Is(ListenedDevice listened) => DeviceType == listened.DeviceType && Id == listened.Id;

    }
}

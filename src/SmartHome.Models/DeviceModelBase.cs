using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Models
{
    public abstract class DeviceModelBase
    {

        public string Id { get; set; }

        public string Name { get; set; }

        public DeviceType DeviceType { get; }

        public DeviceModelBase(DeviceType deviceType)
        {
            DeviceType = deviceType;
        }

        //public abstract bool Is(ListenedDevice device);
        public bool Is(ListenedDevice device) 
            => DeviceType is not DeviceType.None && device.Id == Id && device.DeviceType == DeviceType;


    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SmartHome.Models.SmartContextBase;

namespace SmartHome.Models
{
    public partial class SmartDevices
    {

        private static readonly IEnumerable<ListeningDeviceInfo> _listeningDevices;

        private readonly SmartContextBase _context;

        static SmartDevices()
        {
            _listeningDevices = GetAllDeviceInfos(typeof(SmartDevices), new SmartDevices(null));
        }

        static IEnumerable<ListeningDeviceInfo> GetAllDeviceInfos(Type type,
                                                                  object dummyDevices,
                                                                  string namePrefix = null)
        {
            var infos = type.GetProperties()
                                    .Where(p => p.PropertyType.IsAssignableTo(typeof(RequestableDeviceBase)))
                                    .Select(p =>
                                    {
                                        var obj = (RequestableDeviceBase)p.GetValue(dummyDevices);
                                        return new ListeningDeviceInfo
                                        {
                                            Id = obj.Id,
                                            DeviceType = obj.DeviceType,
                                            Name = UseNamePrefix(p.Name, namePrefix)
                                        };
                                    }).ToList();

            var otherProperties = type.GetProperties()
                                            .Where(p => !p.PropertyType.IsAssignableTo(typeof(RequestableDeviceBase)))
                                            .ToArray();

            foreach (var other in otherProperties)
            {
                var others = GetAllDeviceInfos(other.PropertyType,
                                               other.GetValue(dummyDevices),
                                               UseNamePrefix(other.Name, namePrefix));
                infos.AddRange(others);
            }

            return infos;
        }

        static string UseNamePrefix(string name, string prefix) => prefix is null ? name : $"{prefix} - {name}";

        public static string GetListeningDeviceName(ListenedDevice device) => GetListeningDeviceName(device.Id, device.DeviceType);

        public static string GetListeningDeviceName(string id, DeviceType deviceType) 
            => _listeningDevices.SingleOrDefault(l => l.Id == id && l.DeviceType == deviceType)?.Name;

        public SmartDevices(SmartContextBase context)
        {
            _context = context;
            InitDevices();
        }

        partial void InitDevices();

        class ListeningDeviceInfo
        {
            public string Id { get; set; }

            public DeviceType DeviceType { get; set; }

            public string Name { get; set; }
        }

    }
}

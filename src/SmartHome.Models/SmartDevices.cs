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
            var dummyDevices = new SmartDevices(null);

            _listeningDevices = typeof(SmartDevices)
                                            .GetProperties()
                                            .Where(p => p.PropertyType.IsAssignableTo(typeof(RequestableDeviceBase)))
                                            .Select(p =>
                                            {
                                                var obj = (RequestableDeviceBase) p.GetValue(dummyDevices);
                                                return new ListeningDeviceInfo
                                                {
                                                    Id = obj.Id,
                                                    DeviceType = obj.DeviceType,
                                                    Name = p.Name
                                                };
                                            })
                                            .ToArray();
        }

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

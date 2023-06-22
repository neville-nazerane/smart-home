using SmartHome.Models;
using SmartHome.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.ServerServices.Clients
{
    public static class HttpClientUtils
    {

        public static Task<HttpResponseMessage> StreamEventAsync(this IPhilipsHueClient client, CancellationToken cancellationToken = default)
            => ((PhilipsHueClient)client).StreamEventAsync(cancellationToken);

        public static string GetIp(this IBondClient client) => ((BondClient) client).GetIp();

        public static Task<DeviceType> GetDeviceTypeAsync(this IBondClient client, string id, CancellationToken cancellationToken = default) 
            => ((BondClient) client).GetDeviceTypeAsync(id, cancellationToken);

    }
}

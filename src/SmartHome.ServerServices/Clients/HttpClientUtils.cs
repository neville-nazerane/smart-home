using SmartHome.Models.ClientContracts;
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

    }
}

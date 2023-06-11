using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.ServerServices.Clients
{
    public class BondClient
    {
        private readonly HttpClient _client;

        public BondClient(HttpClient client)
        {
            _client = client;
        }

        public static HttpClient SetupClient(HttpClient client,
                                             string baseUrl,
                                             string token)
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Add("BOND-Token", token);
            return client;
        }


    }
}

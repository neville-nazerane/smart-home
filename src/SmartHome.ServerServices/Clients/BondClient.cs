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

    }
}

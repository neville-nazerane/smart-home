using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.BackgroundProcessor.Services
{
    public class ApiConsumer
    {
        private readonly HttpClient _client;

        public ApiConsumer(HttpClient client)
        {
            _client = client;
        }



    }
}

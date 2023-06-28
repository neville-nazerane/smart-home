using SmartHome.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.ServerServices.Clients
{
    public class HueSyncClient : IHueSyncClient
    {
        private readonly HttpClient _httpClient;

        public HueSyncClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public static void SetupClient(HttpClient client, string baseUrl, string token)
        {
            client.BaseAddress = new(baseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public static HttpClientHandler GetHandler()
        {
            return new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback = (m, cert, cetChain, err) => true
            };
        }



    }
}

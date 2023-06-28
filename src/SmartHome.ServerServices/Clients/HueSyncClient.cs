using SmartHome.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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

        public async Task<bool> GetSyncStateAsync(CancellationToken cancellationToken = default)
        {
            var res = await GetExecutionAsync(cancellationToken);
            return res.SyncActive;
        }

        public Task SetSyncStateAsync(bool state, CancellationToken cancellationToken = default) 
            => UpdateExecuteAsync(new()
                                    {
                                        SyncActive = state
                                    }, cancellationToken);

        Task<ExecutionResult> GetExecutionAsync(CancellationToken cancellationToken = default)
            => _httpClient.GetFromJsonAsync<ExecutionResult>("api/v1/execution", cancellationToken);

        Task UpdateExecuteAsync(ExecutionRequest request, CancellationToken cancellationToken = default)
        {
            string s = JsonSerializer.Serialize(request);
            var content = new StringContent(s, Encoding.UTF8, "application/json");
            return _httpClient.PutAsync("api/v1/execution", content, cancellationToken);
        }


        class ExecutionResult
        {

            [JsonPropertyName("mode")]
            public string Mode { get; set; }

            [JsonPropertyName("syncActive")]
            public bool SyncActive { get; set; }

            [JsonPropertyName("hdmiActive")]
            public bool HdmiActive { get; set; }

            [JsonPropertyName("hdmiSource")]
            public string HdmiSource { get; set; }

        }

        public class ExecutionRequest
        {

            [JsonPropertyName("syncActive")]
            public bool SyncActive { get; set; }

        }

    }
}

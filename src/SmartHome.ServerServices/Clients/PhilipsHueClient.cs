using SmartHome.Models.ClientContracts;
using SmartHome.Models.PhilipsHue;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static SmartHome.Models.SmartContextBase;

namespace SmartHome.ServerServices.Clients
{
    public class PhilipsHueClient : IPhilipsHueClient
    {
        private readonly HttpClient _httpClient;

        public PhilipsHueClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public static HttpClient SetupClient(HttpClient client,
                                  string baseUrl,
                                  string applicationKey)
        {
            client.BaseAddress = new Uri(baseUrl);
            client.Timeout = Timeout.InfiniteTimeSpan;
            client.DefaultRequestHeaders.Add("hue-application-key", applicationKey);
            return client;
        }

        
        public static HttpClientHandler GetHandler() => new HueHandler();

        public async Task<IEnumerable<LightModel>> GetAllLightsAsync(CancellationToken cancellationToken = default)
        {
            var res = await _httpClient.GetFromJsonAsync<HueData<LightResponse>>("clip/v2/resource/light", cancellationToken);
            return res.Data.Select(r => r.ToModel()).ToList();
        }

        public Task SwitchLightAsync(LightRequestModel request, bool switchOn, CancellationToken cancellationToken = default)
        {
            var model = new LightSwitch
            {
                On = new LightOnOff_OnModel
                {
                    On = switchOn
                }
            };
            return _httpClient.PutAsJsonAsync($"/clip/v2/resource/light/{request.Id}", model, cancellationToken);
        }

        #region models

        class HueData<TModel>
        {

            [JsonPropertyName("data")]
            public IEnumerable<TModel> Data { get; set; }

        }

        class LightSwitch
        {
            [JsonPropertyName("on")]
            public LightOnOff_OnModel On { get; set; }
        }

        class LightOnOff_OnModel
        {
            [JsonPropertyName("on")]
            public bool On { get; set; }
        }

        class LightResponse
        {

            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("on")]
            public LightOnOff_OnModel On { get; set; }

            [JsonPropertyName("metadata")]
            public Metadata Metadata { get; set; }

            public LightModel ToModel()
                => new()
                {
                    Id = Id,
                    IsSwitchedOn = On.On,
                    Name = Metadata.Name
                };
        }

        class Metadata
        {
            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("archetype")]
            public string Archetype { get; set; }

        }



        #endregion

        class HueHandler : HttpClientHandler
        {

            public HueHandler()
            {
                //MaxConnectionsPerServer = 1;
                ClientCertificateOptions = ClientCertificateOption.Manual;
                ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) => true;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                request.Version = new Version(2, 0);
                return base.SendAsync(request, cancellationToken);
            }

        }

    }
}

// FULL LIGHT RESPONSE


//public class LightResponse
//{
//    public string id { get; set; }
//    public string id_v1 { get; set; }
//    public Owner owner { get; set; }
//    public Metadata metadata { get; set; }
//    public On on { get; set; }
//    public Dimming dimming { get; set; }
//    public Dimming_Delta dimming_delta { get; set; }
//    public Color_Temperature color_temperature { get; set; }
//    public Color_Temperature_Delta color_temperature_delta { get; set; }
//    public Color color { get; set; }
//    public Dynamics dynamics { get; set; }
//    public Alert alert { get; set; }
//    public Signaling signaling { get; set; }
//    public string mode { get; set; }
//    public Effects effects { get; set; }
//    public Powerup powerup { get; set; }
//    public string type { get; set; }
//}

//public class Owner
//{
//    public string rid { get; set; }
//    public string rtype { get; set; }
//}

//public class Metadata
//{
//    public string name { get; set; }
//    public string archetype { get; set; }
//}

//public class On
//{
//    public bool on { get; set; }
//}

//public class Dimming
//{
//    public float brightness { get; set; }
//    public float min_dim_level { get; set; }
//}

//public class Dimming_Delta
//{
//}

//public class Color_Temperature
//{
//    public int mirek { get; set; }
//    public bool mirek_valid { get; set; }
//    public Mirek_Schema mirek_schema { get; set; }
//}

//public class Mirek_Schema
//{
//    public int mirek_minimum { get; set; }
//    public int mirek_maximum { get; set; }
//}

//public class Color_Temperature_Delta
//{
//}

//public class Color
//{
//    public Xy xy { get; set; }
//    public Gamut gamut { get; set; }
//    public string gamut_type { get; set; }
//}

//public class Xy
//{
//    public float x { get; set; }
//    public float y { get; set; }
//}

//public class Gamut
//{
//    public Red red { get; set; }
//    public Green green { get; set; }
//    public Blue blue { get; set; }
//}

//public class Red
//{
//    public float x { get; set; }
//    public float y { get; set; }
//}

//public class Green
//{
//    public float x { get; set; }
//    public float y { get; set; }
//}

//public class Blue
//{
//    public float x { get; set; }
//    public float y { get; set; }
//}

//public class Dynamics
//{
//    public string status { get; set; }
//    public string[] status_values { get; set; }
//    public float speed { get; set; }
//    public bool speed_valid { get; set; }
//}

//public class Alert
//{
//    public string[] action_values { get; set; }
//}

//public class Signaling
//{
//    public string[] signal_values { get; set; }
//}

//public class Effects
//{
//    public string[] status_values { get; set; }
//    public string status { get; set; }
//    public string[] effect_values { get; set; }
//}

//public class Powerup
//{
//    public string preset { get; set; }
//    public bool configured { get; set; }
//    public On1 on { get; set; }
//    public Dimming1 dimming { get; set; }
//    public Color1 color { get; set; }
//}

//public class On1
//{
//    public string mode { get; set; }
//    public On2 on { get; set; }
//}

//public class On2
//{
//    public bool on { get; set; }
//}

//public class Dimming1
//{
//    public string mode { get; set; }
//    public Dimming2 dimming { get; set; }
//}

//public class Dimming2
//{
//    public float brightness { get; set; }
//}

//public class Color1
//{
//    public string mode { get; set; }
//    public Color_Temperature1 color_temperature { get; set; }
//}

//public class Color_Temperature1
//{
//    public int mirek { get; set; }
//}

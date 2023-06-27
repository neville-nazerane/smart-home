using SmartHome.Models.Contracts;
using SmartHome.Models.PhilipsHue;
using SmartHome.ServerServices.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Reflection.PortableExecutable;
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

        
        public static HttpClientHandler CreateHandler() => new HueHandler();

        #region Light
        
        public async Task<IEnumerable<LightModel>> GetAllLightsAsync(CancellationToken cancellationToken = default)
        {
            var res = await _httpClient.GetFromJsonAsync<HueData<LightResponse>>("clip/v2/resource/light", cancellationToken);
            return res.Data.Select(r => r.ToModel()).ToList();
        }

        public Task SwitchLightAsync(string id, bool switchOn, CancellationToken cancellationToken = default)
        {
            var model = new LightSwitch
            {
                On = new LightOnOff_OnModel
                {
                    On = switchOn
                }
            };
            return _httpClient.PutAsJsonAsync($"/clip/v2/resource/light/{id}", model, cancellationToken);
        }

        public Task SetLightColorAsync(string id, string colorHex, CancellationToken cancellationToken = default)
        {
            var model = new ColorModel
            {
                Color = ConvertHexToColorObject(colorHex)
            };
            return _httpClient.PutAsJsonAsync($"/clip/v2/resource/light/{id}", model, cancellationToken);
        }

        public Task SetBrightnessAsync(string id, double percent, CancellationToken cancellationToken = default)
        {
            var model = new DimmingModel
            {
                Dimming = new()
                {
                    Brightness = percent
                }
            };
            return _httpClient.PutAsJsonAsync($"/clip/v2/resource/light/{id}", model, cancellationToken);
        }


        public async Task<LightModel> GetLightAsync(string id, CancellationToken cancellationToken = default)
        {
            var res = await _httpClient.GetFromJsonAsync<HueData<LightResponse>>($"clip/v2/resource/light/{id}", cancellationToken);
            return res.Data.SingleOrDefault().ToModel();
        }

        #endregion

        public async Task<ButtonModel> GetButtonAsync(string id, CancellationToken cancellationToken = default)
        {
            var res = await _httpClient.GetFromJsonAsync<HueData<ButtonResponse>>($"clip/v2/resource/button/{id}", cancellationToken);
            return res.Data.SingleOrDefault().ToModel();
        }

        public async Task<RotaryModel> GetRotaryAsync(string id, CancellationToken cancellationToken = default)
        {
            var res = await _httpClient.GetFromJsonAsync<HueData<RotaryResponse>>($"clip/v2/resource/relative_rotary/{id}", cancellationToken);
            return res.Data.SingleOrDefault().ToModel();
        }

        #region Motion Sensor

        public async Task<IEnumerable<MotionModel>> GetAllMotionSensorsAsync(CancellationToken cancellationToken = default)
        {
            var res = await _httpClient.GetFromJsonAsync<HueData<MotionResponse>>("clip/v2/resource/motion", cancellationToken);
            var devices = await GetAllDevicesAsync(cancellationToken);

            var models = new List<MotionModel>();

            foreach (var item in res.Data)
            {
                var model = item.ToModel();
                var device = devices.Single(d => d.Id == item.Owner.Rid);
                model.Name = device.Metadata.Name;
                models.Add(model);
            }
            return models;
        }

        public async Task<MotionModel> GetMotionSensorAsync(string id, CancellationToken cancellationToken = default)
        {
            var res = await _httpClient.GetFromJsonAsync<HueData<MotionResponse>>($"clip/v2/resource/motion/{id}", cancellationToken);
            var data = res.Data.SingleOrDefault();
            var device = await GetDeviceAsync(data.Owner.Rid, cancellationToken);
            var model = data.ToModel();
            model.Name = device.Metadata.Name;

            return model;
        }

        #endregion

        public Task<HttpResponseMessage> StreamEventAsync(CancellationToken cancellationToken = default)
            => _httpClient.GetAsync("eventstream/clip/v2", cancellationToken);


        #region Devices

        async Task<IEnumerable<DeviceResponse>> GetAllDevicesAsync(CancellationToken cancellationToken = default)
        {
            var res = await _httpClient.GetFromJsonAsync<HueData<DeviceResponse>>("clip/v2/resource/device", cancellationToken);
            return res.Data;
        }

        async Task<DeviceResponse> GetDeviceAsync(string id, CancellationToken cancellationToken = default)
        {
            var res = await _httpClient.GetFromJsonAsync<HueData<DeviceResponse>>($"clip/v2/resource/device/{id}", cancellationToken);
            return res.Data.SingleOrDefault();
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
                return RetryUtil.Setup(3, TimeSpan.FromSeconds(1))
                                .ExecuteAsync(() => base.SendAsync(request, cancellationToken));
            }

        }

        static string ConvertColorObjectToHex(ColorInfo colorObject)
        {
            if (colorObject is null) return null;
            double normalizedRed = colorObject.XY.X;
            double normalizedGreen = colorObject.XY.Y;
            double normalizedBlue = colorObject.Gamut.Blue.Y;

            // Convert the normalized RGB values to the range 0-255
            int red = (int)(normalizedRed * 255);
            int green = (int)(normalizedGreen * 255);
            int blue = (int)(normalizedBlue * 255);

            // Create the hex color string
            string hexColor = $"#{red:X2}{green:X2}{blue:X2}";

            return hexColor;
        }

        static ColorInfo ConvertHexToColorObject(string hexColor)
        {
            // Remove the '#' character if present
            if (hexColor.StartsWith("#"))
                hexColor = hexColor[1..];

            // Parse the hexadecimal color values
            int red = int.Parse(hexColor[0..2], System.Globalization.NumberStyles.HexNumber);
            int green = int.Parse(hexColor[2..4], System.Globalization.NumberStyles.HexNumber);
            int blue = int.Parse(hexColor[4..6], System.Globalization.NumberStyles.HexNumber);

            // Normalize the RGB values to the range 0-1
            double normalizedRed = red / 255.0;
            double normalizedGreen = green / 255.0;
            double normalizedBlue = blue / 255.0;

            // Create the ColorObject with the converted values
            var colorObject = new ColorInfo
            {
                XY = new XYCoordinates { X = normalizedRed, Y = normalizedGreen },
                Gamut = new GamutCoordinates
                {
                    Red = new XYCoordinates { X = normalizedRed, Y = normalizedGreen },
                    Green = new XYCoordinates { X = normalizedGreen, Y = normalizedBlue },
                    Blue = new XYCoordinates { X = normalizedBlue, Y = normalizedRed }
                },
                GamutType = "C"
            };

            return colorObject;
        }


        #region Models


        class HueData<TModel>
        {

            [JsonPropertyName("data")]
            public IEnumerable<TModel> Data { get; set; }

        }

        class DeviceResponse
        {
            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("metadata")]
            public Metadata Metadata { get; set; }

        }

        public class Owner
        {

            [JsonPropertyName("rid")]
            public string Rid { get; set; }

            [JsonPropertyName("rtype")]
            public string Rtype { get; set; }
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

            [JsonPropertyName("color")]
            public ColorInfo Color { get; set; }

            [JsonPropertyName("dimming")]
            public DimmingInfo Dimming { get; set; }

            public LightModel ToModel()
                => new()
                {
                    Id = Id,
                    IsSwitchedOn = On.On,
                    Name = Metadata.Name,
                    ColorHex = ConvertColorObjectToHex(Color),
                    Brightness = Dimming?.Brightness ?? 0
                };
        }

        class ButtonResponse
        {

            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("button")]
            public ButtonButton Button { get; set; }

            public ButtonModel ToModel()
                => new()
                {
                    Id = Id,
                    LastEvent = Button?.LastEvent,
                    LastEventExecutedOn = Button?.ButtonReport?.Updated
                };

        }

        class ButtonButton
        {
            
            [JsonPropertyName("last_event")]
            public string LastEvent { get; set; }

            [JsonPropertyName("button_report")]
            public ButtonReport ButtonReport { get; set; }

        }


        class ButtonReport
        {

            [JsonPropertyName("updated")]
            public DateTime Updated { get; set; }

        }

        class Metadata
        {
            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("archetype")]
            public string Archetype { get; set; }

        }

        public class RotaryResponse
        {
            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("relative_rotary")]
            public RelativeRotary RelativeRotary { get; set; }

            public RotaryModel ToModel()
                => new()
                {
                    Id = Id,
                    IsLastRotatedClockWise = RelativeRotary?.RotaryReport?.Rotation?.Direction == "clock_wise",
                    Steps = (RelativeRotary?.RotaryReport?.Rotation?.Steps ?? 0) / 15,
                    LastUpdated = RelativeRotary?.RotaryReport?.Updated
                };
        }

        public class RelativeRotary
        {
            [JsonPropertyName("rotary_report")]
            public RotaryReport RotaryReport { get; set; }
        }

        public class RotaryReport
        {
            [JsonPropertyName("updated")]
            public DateTime Updated { get; set; }

            [JsonPropertyName("rotation")]
            public Rotation Rotation { get; set; }
        }

        public class Rotation
        {
            [JsonPropertyName("direction")]
            public string Direction { get; set; }
            public int Steps { get; set; }
        }

        class MotionResponse
        {

            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("motion")]
            public Motion_OnModel Motion { get; set; }

            [JsonPropertyName("owner")]
            public Owner Owner { get; set; }

            public MotionModel ToModel()
                => new()
                {
                    Id = Id,
                    IsMotionDetected = Motion.Motion,
                    LastChanged = Motion.MotionReport.Changed
                };

        }

        class Motion_OnModel
        {

            [JsonPropertyName("motion")]
            public bool Motion { get; set; }

            [JsonPropertyName("motion_report")]
            public MotionReport MotionReport { get; set; }

        }

        class MotionReport
        {
            
            [JsonPropertyName("changed")]
            public DateTime Changed { get; set; }

        }

        class ColorModel
        {
            [JsonPropertyName("color")]
            public ColorInfo Color { get; set; }

        }

        class DimmingModel
        {

            [JsonPropertyName("dimming")]
            public DimmingInfo Dimming { get; set; }

        }

        class DimmingInfo
        {

            [JsonPropertyName("brightness")]
            public double Brightness { get; set; }

        }


        public class XYCoordinates
        {
            [JsonPropertyName("x")]
            public double X { get; set; }

            [JsonPropertyName("y")]
            public double Y { get; set; }
        }

        public class GamutCoordinates
        {
            [JsonPropertyName("red")]
            public XYCoordinates Red { get; set; }

            [JsonPropertyName("green")]
            public XYCoordinates Green { get; set; }

            [JsonPropertyName("blue")]
            public XYCoordinates Blue { get; set; }
        }

        public class ColorInfo
        {
            [JsonPropertyName("xy")]
            public XYCoordinates XY { get; set; }

            [JsonPropertyName("gamut")]
            public GamutCoordinates Gamut { get; set; }

            [JsonPropertyName("gamut_type")]
            public string GamutType { get; set; }
        }

        #endregion

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

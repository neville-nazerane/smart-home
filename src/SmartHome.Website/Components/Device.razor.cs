using Microsoft.AspNetCore.Components;
using SmartHome.ClientServices;
using SmartHome.Models;
using HueModels = SmartHome.Models.PhilipsHue;

namespace SmartHome.Website.Components
{
    public partial class Device
    {

        bool isIdShown = false;

        [Parameter]
        public DeviceModelBase DeviceModel { get; set; }

        [Inject]
        public SmartContext SmartContext { get; set; }

        Task HueColorSetAsync(HueModels.LightModel model, ChangeEventArgs args)
        {
            string color = (string)args.Value;
            return SmartContext.Request(model).SetColorAsync(color);
        }

        Task HueBrightnessSetAsync(HueModels.LightModel model, ChangeEventArgs args)
        {
            double percent = double.Parse((string)args.Value);
            return SmartContext.Request(model).SetBrightnessAsync(percent);
        }

    }
}

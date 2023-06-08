using Microsoft.AspNetCore.Components;
using SmartHome.ClientServices;
using SmartHome.Models;
using SmartHome.Website.Utilities;

using HueModels = SmartHome.Models.PhilipsHue;

namespace SmartHome.Website.Pages
{
    public partial class Index
    {
        private List<DeviceModelBase> devices;

        [Inject]
        public SmartContext Context { get; set; }

        [Inject]
        public ChangeListener ChangeListener { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ChangeListener.OnDeviceChanged += DeviceChanged;
            devices = await Context.FetchAllDevicesAsync().ToThatTypeAsync();
        }

        private async void DeviceChanged(object sender, ChangeListener.DeviceChangedArgs e)
        {
            Console.WriteLine($"the type {e.Info.Type} with ID {e.Info.Id}");
            DeviceModelBase updatedDevice = null;
            int i = 0; 
            for (; i < devices.Count; i++)
            {
                var device = devices[i];
                if (device.Id != e.Info.Id) continue;

                if (device is HueModels.LightModel lightModel && e.Info.Type == DeviceChangedNotify.DeviceType.HueLight)
                {
                    updatedDevice = await Context.MakeRequest(lightModel).GetAsync();
                    break;
                }
                else if (device is HueModels.MotionModel motionModel && e.Info.Type == DeviceChangedNotify.DeviceType.HueMotion)
                {
                    updatedDevice = await Context.MakeRequest(motionModel).GetAsync();
                    break;
                }

            }

            if (updatedDevice is not null)
            {
                devices[i] = updatedDevice;
                StateHasChanged();
            }
        }

    }
}

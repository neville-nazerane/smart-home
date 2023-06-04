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


            //var index = devices.FindIndex(d => DeviceEquals(d, e));
            //var device = devices.ElementAtOrDefault(index);
            //if (device is not null)
            //{
            //    await Context.MakeRequest(device).GetAsync();
            //}
            //Console.WriteLine($"A device has changed with ID {e.Info.Id} and type {e.Info.Type}");
        }

        //Task UpdateDeviceIfExistAsync(DeviceModelBase device, ChangeListener.DeviceChangedArgs e)
        //{
        //    if (e.Info.Id != device.Id) return false;

        //    if (device is HueModels.LightModel && e.Info.Type == DeviceChangedNotify.DeviceType.HueLight)
        //        return true;

        //    return false;
        //}

    }
}

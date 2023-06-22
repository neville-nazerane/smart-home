using Microsoft.AspNetCore.Components;
using SmartHome.ClientServices;
using SmartHome.Models;
using SmartHome.Website.Utilities;

using HueModels = SmartHome.Models.PhilipsHue;
using BondModels = SmartHome.Models.Bond;

namespace SmartHome.Website.Pages
{
    public partial class Index : IDisposable
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
            Console.WriteLine($"the type {e.Info.DeviceType} with ID {e.Info.Id}");

            var updateIndex = devices.FindIndex(d => d.Is(e.Info));
            var device = devices.ElementAtOrDefault(updateIndex);
            if (device is not null)
            {
                devices[updateIndex] = await GetUpdatedDeviceAsync(device);
                StateHasChanged();
            }
        }

        async Task<DeviceModelBase> GetUpdatedDeviceAsync(DeviceModelBase device)
        {
            return device switch
            {
                HueModels.LightModel lightModel => await Context.Request(lightModel).GetAsync(),
                HueModels.MotionModel motionModel => await Context.Request(motionModel).GetAsync(),
                BondModels.CeilingFanModel fanModel => await Context.Request(fanModel).GetAsync(),
                BondModels.RollerModel rollerModel => await Context.Request(rollerModel).GetAsync(),
                _ => throw new Exception($"Couldn't request for type {device.GetType()}"),
            };
        }


        public void Dispose()
        {
            InnerDispose();
            GC.SuppressFinalize(this);
        }

        ~Index()
        {
            InnerDispose();
        }

        void InnerDispose()
        {
            ChangeListener.OnDeviceChanged += DeviceChanged;
        }

    }
}

using Microsoft.AspNetCore.Components;
using SmartHome.ClientServices;
using SmartHome.Models;

namespace SmartHome.Website.Pages
{
    public partial class Index
    {
        private IEnumerable<DeviceModelBase> res;

        [Inject]
        public SmartContext Context { get; set; }

        [Inject]
        public ChangeListener ChangeListener { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ChangeListener.OnDeviceChanged += DeviceChanged;
            res = await Context.FetchAllDevicesAsync();
        }

        private void DeviceChanged(object sender, ChangeListener.DeviceChangedArgs e)
        {
            Console.WriteLine($"A device has changed with ID {e.Info.Id} and type {e.Info.Type}");
        }
    }
}

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

        protected override async Task OnInitializedAsync()
        {
            res = await Context.FetchAllDevicesAsync();
        }

    }
}

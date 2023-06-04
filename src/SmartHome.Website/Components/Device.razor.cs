using Microsoft.AspNetCore.Components;
using SmartHome.ClientServices;
using SmartHome.Models;

namespace SmartHome.Website.Components
{
    public partial class Device
    {

        [Parameter]
        public DeviceModelBase DeviceModel { get; set; }

        [Inject]
        public SmartContext SmartContext { get; set; }

    }
}

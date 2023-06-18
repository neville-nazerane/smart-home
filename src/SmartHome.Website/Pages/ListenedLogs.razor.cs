using Microsoft.AspNetCore.Components;
using SmartHome.ClientServices;
using SmartHome.Models;

namespace SmartHome.Website.Pages
{
    public partial class ListenedLogs
    {

        private IEnumerable<DeviceLog> logs;

        [Inject]
        public SmartContext SmartContext { get; set; }

        protected override async Task OnInitializedAsync()
        {
            logs = await SmartContext.GetListeningLogsAsync(1, 50);
        }

    }
}

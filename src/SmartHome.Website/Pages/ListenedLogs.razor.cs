using Microsoft.AspNetCore.Components;
using SmartHome.ClientServices;
using SmartHome.Models;

namespace SmartHome.Website.Pages
{
    public partial class ListenedLogs
    {

        private List<DeviceLog> logs = new();

        [Inject]
        public SmartContext SmartContext { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await foreach (var log in SmartContext.GetListeningLogsAsync(50, 1))
                logs.Add(log);
        }

    }
}

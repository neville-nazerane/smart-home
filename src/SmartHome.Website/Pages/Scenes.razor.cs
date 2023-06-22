using Microsoft.AspNetCore.Components;
using SmartHome.ClientServices;
using SmartHome.Models;

namespace SmartHome.Website.Pages
{
    public partial class Scenes
    {
        private IEnumerable<Scene> scenes;

        [Inject]
        public SmartContext SmartContext { get; set; }


        protected override async Task OnInitializedAsync()
        {
            scenes = await SmartContext.Scenes.GetAllAsync();
        }

    }
}

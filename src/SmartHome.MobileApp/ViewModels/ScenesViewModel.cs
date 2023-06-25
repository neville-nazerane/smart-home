using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartHome.ClientServices;
using SmartHome.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.MobileApp.ViewModels
{
    public partial class ScenesViewModel : ObservableObject
    {

        [ObservableProperty]
        IEnumerable<Scene> scenes;

        private readonly SmartContext _context;

        public ScenesViewModel(SmartContext context)
        {
            _context = context;
        }

        public async Task InitAsync()
        {
            Scenes = await _context.Scenes.GetAllScenesAsync();
        }

    }

}

using SmartHome.ClientServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.MobileApp.ViewModels
{
    public class ScenesViewModel
    {
        private readonly SmartContext _context;

        public ScenesViewModel(SmartContext context)
        {
            _context = context;
        }

        public async Task InitAsync()
        {
            var scenes = await _context.Scenes.GetAllScenesAsync();
        }

    }

}

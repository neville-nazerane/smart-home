using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SmartHome.Models.SmartContextBase;

namespace SmartHome.Models
{
    public partial class SmartDevices
    {
        private readonly SmartContextBase _context;

        public SmartDevices(SmartContextBase context)
        {
            _context = context;
        }

    }
}

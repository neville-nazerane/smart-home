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

        public HueButtonRequestModel HueClosetButton { get; private set; }

        public HueMotionRequestModel ClosetMotionSensor { get; private set; }

        public HueLightRequestModel ClosetLight { get; private set; }

        partial void InitDevices()
        {
            ClosetMotionSensor = new(_context, "c8cc0112-0f5f-4559-9d7e-11a6e01f85b2");
            ClosetLight = new(_context, "d5a8f6ad-951a-4f48-a937-4cb436100409");
            HueClosetButton = new(_context, "419bf6d0-02d5-4932-bc03-b761c9ecbb71");
        }

    }
}

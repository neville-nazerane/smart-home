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

        public HueDimmerSwitch BedroomControl { get; private set; }

        public BondCeilingFanRequestModel BedroomCeilingFan { get; private set; }



        public HueLightRequestModel ComputerLightPlug { get; private set; }

        public HueLightRequestModel ComputerLeftBar { get; private set; }

        public HueLightRequestModel ComputerRightBar { get; private set; }

        public HueLightRequestModel ComputerLeftIris { get; private set; }

        public HueLightRequestModel ComputerRightIris { get; private set; }


        partial void InitDevices()
        {
            ComputerLightPlug = new(_context, "56177214-d8b7-4d05-bb27-119538131e2f");
            ComputerLeftBar = new(_context, "6980ed6c-d4fc-4e52-a2d2-8ff72b7bbb5e");
            ComputerRightBar = new(_context, "14d8fd8b-f454-4dca-87aa-d9164bbe310c");
            ComputerLeftIris = new(_context, "d7bf7254-f2d8-4d6f-8b29-1e5cebe3949d");
            ComputerRightIris = new(_context, "4bf45461-f3de-4476-ba58-d5c2c31d5816");

            ClosetMotionSensor = new(_context, "c8cc0112-0f5f-4559-9d7e-11a6e01f85b2");
            ClosetLight = new(_context, "d5a8f6ad-951a-4f48-a937-4cb436100409");
            HueClosetButton = new(_context, "419bf6d0-02d5-4932-bc03-b761c9ecbb71");
            BedroomCeilingFan = new(_context, "20e72b75");

            BedroomControl = new(_context,
                                 "0f7ed57c-7df9-41d5-b8bc-aa1ef244ff99",
                                 "b2e0d9ac-dc4a-4f4e-aea3-d38edf78b76d",
                                 "c3e07c4d-6ecf-4575-9aa9-c18013aa7ffb",
                                 "33fbad67-0ce8-462a-8f7b-ecf47b58d55d");
        }

        public class HueDimmerSwitch
        {
            public HueButtonRequestModel OnOffButton { get; private set; }
            public HueButtonRequestModel IncreaseButton { get; private set; }
            public HueButtonRequestModel DecreaseButton { get; private set; }
            public HueButtonRequestModel HueButton { get; private set; }


            public HueDimmerSwitch(SmartContextBase context, 
                                   string onOffButtonId,
                                   string increaseButtonId,
                                   string decreaseButtonId,
                                   string hueButtonId)
            {
                OnOffButton = new(context, onOffButtonId);
                IncreaseButton = new(context, increaseButtonId);
                DecreaseButton = new(context, decreaseButtonId);
                HueButton = new(context, hueButtonId);
            }

        }

    }
}

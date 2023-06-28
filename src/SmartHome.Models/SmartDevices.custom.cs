using SmartHome.Models.Contracts;
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

        #region Closet

        public HueButtonRequestModel HueClosetButton { get; private set; }

        public HueMotionRequestModel ClosetMotionSensor { get; private set; }

        public HueLightRequestModel ClosetLight { get; private set; }


        #endregion



        #region Bedroom

        public HueDimmerSwitch BedroomControl { get; private set; }

        public BondCeilingFanRequestModel BedroomCeilingFan { get; private set; }


        public HueLightRequestModel ComputerLightPlug { get; private set; }

        public HueLightRequestModel ComputerLeftBar { get; private set; }

        public HueLightRequestModel ComputerRightBar { get; private set; }

        public HueLightRequestModel ComputerLeftIris { get; private set; }

        public HueLightRequestModel ComputerRightIris { get; private set; }

        public HueButtonRequestModel ComputerButton { get; private set; }

        public HueMotionRequestModel BedroomMotionSensor1 { get; private set; }
        public HueMotionRequestModel BedroomMotionSensor2 { get; private set; }


        #endregion


        #region Front

        public IHueSyncClient HueSync => _context.HueSyncClient;

        public BondRollerRequestModel KitchenRoller { get; private set; }

        public HueMotionRequestModel FrontMotionSensor { get; private set; }

        public BondCeilingFanRequestModel FrontCeilingFan { get; private set; }

        public HueLightRequestModel MiddleLight { get; private set; }

        public HueDimmerSwitch FrontControl { get; private set; }
        
        public HueDial FrontDial { get; private set; }

        public HueLightRequestModel TvLeftBar { get; set; }
        public HueLightRequestModel TvRightBar { get; set; }
        public HueLightRequestModel TvLight { get; set; }
        public HueLightRequestModel TvBottomLightStrip { get; set; }


        #endregion



        #region Kitchen

        public SwitchBotRequestModel KitchenLight2 { get; private set; }

        public SwitchBotRequestModel KitchenLight1 { get; private set; }

        public SwitchBotRequestModel InsectSwitch { get; private set; }

        public HueLightRequestModel InsectPower { get; private set; }


        #endregion


        partial void InitDevices()
        {

            // bedroom
            BedroomMotionSensor1 = new(_context, "749b6ec1-bc35-4f46-b11d-4f4b77d1605a");
            BedroomMotionSensor2 = new(_context, "9eb3233b-21c5-4fbe-9def-61474abc6e6e");

            ComputerLightPlug = new(_context, "56177214-d8b7-4d05-bb27-119538131e2f");
            ComputerLeftBar = new(_context, "6980ed6c-d4fc-4e52-a2d2-8ff72b7bbb5e");
            ComputerRightBar = new(_context, "14d8fd8b-f454-4dca-87aa-d9164bbe310c");
            ComputerLeftIris = new(_context, "d7bf7254-f2d8-4d6f-8b29-1e5cebe3949d");
            ComputerRightIris = new(_context, "4bf45461-f3de-4476-ba58-d5c2c31d5816");
            ComputerButton = new(_context, "d5596781-7952-46f3-9b08-4d8d0110f940");

            ClosetMotionSensor = new(_context, "c8cc0112-0f5f-4559-9d7e-11a6e01f85b2");
            ClosetLight = new(_context, "d5a8f6ad-951a-4f48-a937-4cb436100409");
            HueClosetButton = new(_context, "419bf6d0-02d5-4932-bc03-b761c9ecbb71");
            BedroomCeilingFan = new(_context, "20e72b75");

            BedroomControl = new(_context,
                                 "0f7ed57c-7df9-41d5-b8bc-aa1ef244ff99",
                                 "b2e0d9ac-dc4a-4f4e-aea3-d38edf78b76d",
                                 "c3e07c4d-6ecf-4575-9aa9-c18013aa7ffb",
                                 "33fbad67-0ce8-462a-8f7b-ecf47b58d55d");


            // kitchen 
            KitchenLight1 = new(_context, "0d6a32dc-d772-45f9-96c8-96669004b1c8");
            KitchenLight2 = new(_context, "235be111-09d2-40f5-8a30-3b185aebcf4e");
            InsectSwitch = new(_context, "0c379227-dd37-48e5-af34-4a8e43396278");
            InsectPower = new(_context, "c252f48e-83a2-4788-8edc-7fd9a82d3154");

            // front
            FrontMotionSensor = new(_context, "b37b3923-e905-42d1-9b20-ef19a5ebdbae");
            KitchenRoller = new(_context, "e2a857cfe29c2a53");
            FrontControl = new(_context,
                               "1e22fc91-d3d3-4051-895e-6590a32ed339",
                               "6e63013c-5b2e-42ef-b456-549a676c47de",
                               "4e329e07-2822-43c0-9a5e-a543141f8e12",
                               "f805f6c1-1fe5-4301-814d-0d93b85a086f");
            FrontDial = new(_context,
                            "4c7244ad-f79b-49df-9071-29513953f89d",
                            "e5d1f28c-68fe-4507-ac8f-50066552aff3",
                            "912e79bb-de5e-49ba-892d-370757841ba9",
                            "88b2afa5-9f56-4285-b224-449d1506285e",
                            "2986dc69-87b8-41c4-9275-6c79a1a6bd06");
            FrontCeilingFan = new(_context, "77f2be51");
            MiddleLight = new(_context, "ff9e4968-20f7-41f4-8bf3-3e045564896c");
            TvLeftBar = new(_context, "8f0514bf-2bf7-43b9-8a73-d7e88fe92eae");
            TvRightBar = new(_context, "1ac9fcf2-b3ca-4f6a-9747-8e72222c196c");
            TvLight = new(_context, "23fc03a9-b8ab-4e91-b710-af106b2a25b0");
            TvBottomLightStrip = new(_context, "0de87304-c438-4c6e-951b-2cd9274d54b9");

        }

        public class HueDial
        {

            public HueButtonRequestModel One { get; private set; }
            public HueButtonRequestModel Two { get; private set; }
            public HueButtonRequestModel Three { get; private set; }
            public HueButtonRequestModel Four { get; private set; }
            public HueRotaryRequestModel Rotary { get; private set; }


            public HueDial(SmartContextBase context,
                            string onOffButtonId,
                            string increaseButtonId,
                            string decreaseButtonId,
                            string hueButtonId,
                            string rotaryId)
            {
                One = new(context, onOffButtonId);
                Two = new(context, increaseButtonId);
                Three = new(context, decreaseButtonId);
                Four = new(context, hueButtonId);
                Rotary = new(context, rotaryId);
            }

            public static bool operator ==(ListenedDevice device, HueDial self)
                => device == self.One || device == self.Two || device == self.Three || device == self.Four || device == self.Rotary;

            public static bool operator !=(ListenedDevice device, HueDial self)
                => device != self.One && device != self.Two && device != self.Three && device != self.Four && device != self.Rotary;

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                return base.Equals(obj);
            }
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

            public static bool operator ==(ListenedDevice device, HueDimmerSwitch self)
                => device == self.OnOffButton || device == self.IncreaseButton || device == self.DecreaseButton || device == self.HueButton;

            public static bool operator !=(ListenedDevice device, HueDimmerSwitch self)
                => device != self.OnOffButton && device != self.IncreaseButton && device != self.DecreaseButton && device != self.HueButton;

            public override bool Equals(object obj)
            {
                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

        }

    }
}

﻿using Microsoft.AspNetCore.Components;
using SmartHome.ClientServices;
using SmartHome.Models;
using SmartHome.Website.Utilities;

using HueModels = SmartHome.Models.PhilipsHue;
using BondModels = SmartHome.Models.Bond;

namespace SmartHome.Website.Pages
{
    public partial class Index
    {
        private List<DeviceModelBase> devices;

        [Inject]
        public SmartContext Context { get; set; }

        [Inject]
        public ChangeListener ChangeListener { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ChangeListener.OnDeviceChanged += DeviceChanged;
            devices = await Context.FetchAllDevicesAsync().ToThatTypeAsync();
        }

        private async void DeviceChanged(object sender, ChangeListener.DeviceChangedArgs e)
        {
            Console.WriteLine($"the type {e.Info.DeviceType} with ID {e.Info.Id}");
            DeviceModelBase updatedDevice = null;
            int i = 0; 
            for (; i < devices.Count; i++)
            {
                var device = devices[i];
                if (device.Id != e.Info.Id) continue;

                if (device is HueModels.LightModel lightModel && e.Info.DeviceType == DeviceType.HueLight)
                {
                    updatedDevice = await Context.Request(lightModel).GetAsync();
                    break;
                }
                else if (device is HueModels.MotionModel motionModel && e.Info.DeviceType == DeviceType.HueMotion)
                {
                    updatedDevice = await Context.Request(motionModel).GetAsync();
                    break;
                }
                else if (device is BondModels.CeilingFanModel fanModel && e.Info.DeviceType == DeviceType.BondFan)
                {
                    updatedDevice = await Context.Request(fanModel).GetAsync();
                    break;
                }
                else if (device is BondModels.RollerModel rollerModel && e.Info.DeviceType == DeviceType.BondRoller)
                {
                    updatedDevice = await Context.Request(rollerModel).GetAsync();
                    break;
                }

                // retain index i to update devices
                if (updatedDevice is not null) break;
            }



            if (updatedDevice is not null)
            {
                devices[i] = updatedDevice;
                StateHasChanged();
            }
        }

    }
}

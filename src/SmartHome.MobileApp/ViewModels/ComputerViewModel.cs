using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Graphics;
using SmartHome.ClientServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static SmartHome.Models.SmartContextBase;
using PhilipsHue = SmartHome.Models.PhilipsHue;

namespace SmartHome.MobileApp.ViewModels
{
    public partial class ComputerViewModel : ObservableObject
    {

        [ObservableProperty]
        Color leftHaloColor;

        [ObservableProperty]
        Color rightHaloColor;

        [ObservableProperty]
        Color leftBarColor;

        [ObservableProperty]
        Color rightBarColor;

        [ObservableProperty]
        Color stripColor;
        private readonly SmartContext _smartContext;
        private readonly ChangeListener _changeListener;

        public ComputerViewModel(SmartContext smartContext, ChangeListener changeListener)
        {
            _smartContext = smartContext;
            _changeListener = changeListener;
        }

        [RelayCommand]
        public async Task SetupAsync(CancellationToken cancellationToken = default)
        {
            LeftHaloColor = await GetColorAsync(_smartContext.Devices.ComputerLeftIris.GetAsync(cancellationToken));
            RightHaloColor = await GetColorAsync(_smartContext.Devices.ComputerRightIris.GetAsync(cancellationToken));
            LeftBarColor = await GetColorAsync(_smartContext.Devices.ComputerLeftBar.GetAsync(cancellationToken));
            RightBarColor = await GetColorAsync(_smartContext.Devices.ComputerRightBar.GetAsync(cancellationToken));
            StripColor = await GetColorAsync(_smartContext.Devices.ComputerGradient.GetAsync(cancellationToken));
        }

        [RelayCommand]
        Task SwitchLeftHaloAsync(CancellationToken cancellationToken = default) => SwitchAsync(_smartContext.Devices.ComputerLeftIris, cancellationToken);

        [RelayCommand]
        Task SwitchRightHaloAsync(CancellationToken cancellationToken = default) => SwitchAsync(_smartContext.Devices.ComputerRightIris, cancellationToken);

        [RelayCommand]
        Task SwitchLeftBarAsync(CancellationToken cancellationToken = default) => SwitchAsync(_smartContext.Devices.ComputerLeftBar, cancellationToken);

        [RelayCommand]
        Task SwitchRightBarAsync(CancellationToken cancellationToken = default) => SwitchAsync(_smartContext.Devices.ComputerRightBar, cancellationToken);

        [RelayCommand]
        Task SwitchStripAsync(CancellationToken cancellationToken = default) => SwitchAsync(_smartContext.Devices.ComputerGradient, cancellationToken);


        static async Task SwitchAsync(HueLightRequestModel req, CancellationToken cancellationToken = default)
        {
            var light = await req.GetAsync(cancellationToken);
            await req.TriggerSwitchAsync(!light.IsSwitchedOn, cancellationToken);
        }

        static async Task<Color> GetColorAsync(Task<PhilipsHue.LightModel> task)
        {
            var light = await task;
            return GetColorByState(light.IsSwitchedOn);
        }

        static Color GetColorByState(bool isOn) => isOn ? Colors.GreenYellow : Colors.OrangeRed;

        private async void OnDeviceChanged(object sender, ChangeListener.DeviceChangedArgs e)
        {
            if (e.Info == _smartContext.Devices.ComputerLeftIris)
                LeftHaloColor = await GetColorAsync(_smartContext.Devices.ComputerLeftIris.GetAsync());

            if (e.Info == _smartContext.Devices.ComputerRightIris)
                RightHaloColor = await GetColorAsync(_smartContext.Devices.ComputerRightIris.GetAsync());

            if (e.Info == _smartContext.Devices.ComputerLeftBar)
                LeftBarColor = await GetColorAsync(_smartContext.Devices.ComputerLeftBar.GetAsync());

            if (e.Info == _smartContext.Devices.ComputerRightBar)
                RightBarColor = await GetColorAsync(_smartContext.Devices.ComputerRightBar.GetAsync());

            if (e.Info == _smartContext.Devices.ComputerGradient)
                StripColor = await GetColorAsync(_smartContext.Devices.ComputerGradient.GetAsync());
        }

        internal void Subscribe()
        {
            _changeListener.OnDeviceChanged += OnDeviceChanged;
        }

        internal void Unsubscribe()
        {
            _changeListener.OnDeviceChanged -= OnDeviceChanged;
        }

    }
}

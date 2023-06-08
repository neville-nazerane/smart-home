using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.ServerServices
{
    public class GpioService : IGpioService
    {
        private readonly GpioController _controller;

        public GpioService()
        {
            _controller = new GpioController();
        }

        public bool GetBinaryRead(int pinNumber)
        {
            if (!_controller.IsPinOpen(pinNumber))
                _controller.OpenPin(pinNumber, PinMode.Input);
            return (bool)_controller.Read(pinNumber);
        }

        public Task<bool> GetBinaryReadAsync(int pinNumber) => Task.FromResult(GetBinaryRead(pinNumber));

    }
}

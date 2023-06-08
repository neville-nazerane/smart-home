namespace SmartHome.ServerServices
{
    public interface IGpioService
    {
        Task<bool> GetBinaryReadAsync(int pinNumber);
    }
}
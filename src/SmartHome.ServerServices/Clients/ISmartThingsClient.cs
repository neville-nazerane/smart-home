namespace SmartHome.ServerServices.Clients
{
    public interface ISmartThingsClient
    {
        Task<bool> ExecuteDeviceAsync(string deviceId, SmartThingsClient.DeviceExecuteModel model, CancellationToken cancellationToken = default);
        Task<bool> ExecuteDeviceAsync(string deviceId, SmartThingsClient.DeviceExecuteModel[] models, CancellationToken cancellationToken = default);
    }
}
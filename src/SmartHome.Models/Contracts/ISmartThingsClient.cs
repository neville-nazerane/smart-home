namespace SmartHome.Models.Contracts
{

    public interface ISmartThingsClient
    {
        Task TriggerSwitchBotAsync(string deviceId, bool isOn, CancellationToken cancellationToken = default);
    }

}

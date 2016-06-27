using NServiceBus;
using NServiceBus.Logging;

#region DataResponseMessageHandler
class DataResponseMessageHandler : IHandleMessages<DataResponseMessage>
#endregion
{
    static ILog log = LogManager.GetLogger(typeof(DataResponseMessageHandler));
    public void Handle(DataResponseMessage message)
    {
        log.Info($"Response received with description: {message.String}");
    }
}
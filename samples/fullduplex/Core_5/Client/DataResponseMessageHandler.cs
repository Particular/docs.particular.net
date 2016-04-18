using NServiceBus;
using NServiceBus.Logging;

#region DataResponseMessageHandler

class DataResponseMessageHandler : IHandleMessages<DataResponseMessage>

    #endregion

{
    static ILog log = LogManager.GetLogger<DataResponseMessageHandler>();

    public void Handle(DataResponseMessage message)
    {
        log.InfoFormat("Response received with description: {0}", message.String);
    }
}
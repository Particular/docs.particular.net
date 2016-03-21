using NServiceBus;
using NServiceBus.Logging;

#region DataResponseMessageHandler
class DataResponseMessageHandler : IHandleMessages<DataResponseMessage>
#endregion
{
    static ILog ILog = LogManager.GetLogger(typeof(DataResponseMessageHandler));
    public void Handle(DataResponseMessage message)
    {
        ILog.InfoFormat("Response received with description: {0}", message.String);
    }
}
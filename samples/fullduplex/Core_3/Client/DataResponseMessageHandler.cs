using log4net;
using NServiceBus;

#region DataResponseMessageHandler

class DataResponseMessageHandler : IHandleMessages<DataResponseMessage>

    #endregion

{
    static ILog log = LogManager.GetLogger(typeof(DataResponseMessageHandler));

    public void Handle(DataResponseMessage message)
    {
        log.InfoFormat("Response received with description: {0}", message.String);
    }
}
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region DataResponseMessageHandler
class DataResponseMessageHandler : IHandleMessages<DataResponseMessage>
#endregion
{
    static ILog log = LogManager.GetLogger<DataResponseMessageHandler>();
    public Task Handle(DataResponseMessage message, IMessageHandlerContext context)
    {
        log.InfoFormat("Response received with description: {0}", message.String);
        return Task.FromResult(0);
    }

}
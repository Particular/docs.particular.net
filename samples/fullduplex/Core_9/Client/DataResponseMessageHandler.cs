using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region DataResponseMessageHandler
class DataResponseMessageHandler :
    IHandleMessages<DataResponseMessage>
#endregion
{
    static ILog log = LogManager.GetLogger<DataResponseMessageHandler>();
    public Task Handle(DataResponseMessage message, IMessageHandlerContext context)
    {
        log.Info($"Response received with description: {message.String}");
        return Task.CompletedTask;
    }
}
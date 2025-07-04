using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;

#region DataResponseMessageHandler
class DataResponseMessageHandler(ILogger<DataResponseMessageHandler> logger) :
    IHandleMessages<DataResponseMessage>
#endregion
{
    public Task Handle(DataResponseMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation("Response received with description: {Description}", message.String);
        return Task.CompletedTask;
    }
}
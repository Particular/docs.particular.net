using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using UsingInterfaces.Messages;

#region immutable-messages-as-interface-handling
public class MyMessageAsInterfaceHandler(ILogger<MyMessageAsInterfaceHandler> logger) :
    IHandleMessages<IMyMessage>
{
    public Task Handle(IMyMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation("IMyMessage (as interface) received from server with data: {Data}", message.Data);
        return Task.CompletedTask;
    }
}
#endregion
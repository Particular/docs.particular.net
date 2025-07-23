using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using UsingClasses.Messages;

#region immutable-messages-as-class-handling
public class MyMessageAsClassHandler(ILogger<MyMessageAsClassHandler> logger) :
    IHandleMessages<MyMessage>
{
     public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation("MyMessage (as class) received from server with data: {Data}", message.Data);
        return Task.CompletedTask;
    }
}
#endregion
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

#region MyHandler
public class MyHandler(ILogger<MyHandler> logger):
    IHandleMessages<MyMessage>
{
    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation("Message received. Id: {Id}", message.Id);
        // throw new Exception("Uh oh - something went wrong....");
        return Task.CompletedTask;
    }
}
#endregion

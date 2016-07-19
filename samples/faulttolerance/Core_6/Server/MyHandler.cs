using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region MyHandler
public class MyHandler :
    IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger<MyHandler>();

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        log.Info($"Message received. Id: {message.Id}");
        // throw new Exception("Uh oh - something went wrong....");
        return Task.FromResult(0);
    }
}
#endregion

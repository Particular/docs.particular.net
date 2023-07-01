using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Threading.Tasks;

#region MyHandler
public class MyHandler :
    IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger<MyHandler>();

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        log.Info($"Message received. Id: {message.Id}");
       // throw new ArgumentNullException("Uh oh - something went wrong....");
        // throw new DivideByZeroException("DivideByZeroException - something went wrong....");
        return Task.CompletedTask;
    }
}
#endregion

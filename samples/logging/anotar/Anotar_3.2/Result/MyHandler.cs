using NServiceBus;
using System.Threading.Tasks;
using NServiceBus.Logging;

#region resulthandler

public class MyHandler :
    IHandleMessages<MyMessage>
{
    static ILog logger = LogManager.GetLogger("MyHandler");

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        logger.Info("Method: 'Task Handle(MyMessage, IMessageHandlerContext)'. Line: ~12. Hello from MyHandler");
        return Task.CompletedTask;
    }
}

#endregion
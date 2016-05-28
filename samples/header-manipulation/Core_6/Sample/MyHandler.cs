using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region handler

public class MyHandler : IHandleMessages<MyMessage>
{
    static ILog logger = LogManager.GetLogger<MyHandler>();

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        logger.Info("Hello from MyHandler");
        foreach (var line in context.MessageHeaders.OrderBy(x => x.Key)
            .Select(x => $"Key={x.Key}, Value={x.Value}"))
        {
            logger.Info(line);
        }
        return Task.FromResult(0);
    }
}

#endregion
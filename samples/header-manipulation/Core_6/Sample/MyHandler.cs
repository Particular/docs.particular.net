using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region handler

public class MyHandler : IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger<MyHandler>();

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        log.Info("Hello from MyHandler");
        foreach (var line in context.MessageHeaders.OrderBy(x => x.Key)
            .Select(x => $"Key={x.Key}, Value={x.Value}"))
        {
            log.Info(line);
        }
        return Task.FromResult(0);
    }
}

#endregion
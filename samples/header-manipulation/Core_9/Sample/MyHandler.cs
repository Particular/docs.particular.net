using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
#region handler

public class MyHandler(ILogger<MyHandler> logger) :
    IHandleMessages<MyMessage>
{
    
    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation("Hello from MyHandler");
        var headers = context.MessageHeaders;
        foreach (var line in headers.OrderBy(x => x.Key)
            .Select(x => $"Key={x.Key}, Value={x.Value}"))
        {
            logger.LogInformation(line);
        }
        return Task.CompletedTask;
    }
}

#endregion
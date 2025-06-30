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
        foreach (var header in headers.OrderBy(x => x.Key))
        {
            logger.LogInformation("Header Key={Key}, Value={Value}", header.Key, header.Value);
        }
        return Task.CompletedTask;
    }
}

#endregion
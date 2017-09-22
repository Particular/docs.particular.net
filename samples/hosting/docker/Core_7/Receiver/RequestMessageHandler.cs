using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class RequestMessageHandler
    : IHandleMessages<RequestMessage>
{
    static ILog log = LogManager.GetLogger<RequestMessageHandler>();

    public async Task Handle(RequestMessage message, IMessageHandlerContext context)
    {
        log.Info($"Request received with description: {message.Data}");
        
        await context.Reply(new ResponseMessage
        {
            Id = message.Id,
            Data = message.Data
        });
    }
}
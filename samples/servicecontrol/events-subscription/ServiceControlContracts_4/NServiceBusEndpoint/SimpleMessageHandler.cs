using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class SimpleMessageHandler :
    IHandleMessages<SimpleMessage>
{
    static ILog log = LogManager.GetLogger<SimpleMessageHandler>();

    public Task Handle(SimpleMessage message, IMessageHandlerContext context)
    {
        log.Info($"Received message with Id = {message.Id}.");
        throw new Exception("BOOM!");
    }
}
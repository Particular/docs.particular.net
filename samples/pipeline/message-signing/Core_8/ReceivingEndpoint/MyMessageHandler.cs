using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class MyMessageHandler :
    IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger<MyMessageHandler>();

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        var signature = context.MessageHeaders["X-Message-Signature"];

        log.Info("Handling message...");
        log.Info($"  Contents = {message.Contents}");
        log.Info($"  Signature = {signature}");

        return Task.CompletedTask;
    }
}
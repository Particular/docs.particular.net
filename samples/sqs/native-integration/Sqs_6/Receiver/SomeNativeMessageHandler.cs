using System.Threading.Tasks;
using Amazon.SQS.Model;
using NativeIntegration.Receiver;
using NServiceBus;
using NServiceBus.Logging;

#region HandlerAccessingNativeMessage
public class SomeNativeMessageHandler : IHandleMessages<SomeNativeMessage>
{
    static ILog log = LogManager.GetLogger<SomeNativeMessageHandler>();

    public Task Handle(SomeNativeMessage eventMessage, IMessageHandlerContext context)
    {
        var nativeMessage = context.Extensions.Get<Message>();
        var nativeAttributeFound = nativeMessage.MessageAttributes.TryGetValue("SomeRandomKey", out var randomAttributeKey);

        log.Info($"Received {nameof(SomeNativeMessage)} with message {eventMessage.ThisIsTheMessage}.");

        if (nativeAttributeFound)
        {
            log.Info($"Found attribute 'SomeRandomKey' with value '{randomAttributeKey.StringValue}'");
        }

        return Task.CompletedTask;
    }
}
#endregion
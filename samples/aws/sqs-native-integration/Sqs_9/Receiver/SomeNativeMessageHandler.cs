using Amazon.SQS.Model;
using Microsoft.Extensions.Logging;
using NativeIntegration.Receiver;

#region HandlerAccessingNativeMessage
public class SomeNativeMessageHandler(ILogger<SomeNativeMessageHandler> logger) : IHandleMessages<SomeNativeMessage>
{
    public async Task Handle(SomeNativeMessage eventMessage, IMessageHandlerContext context)
    {
        var nativeMessage = context.Extensions.Get<Message>();
        var nativeAttributeFound = nativeMessage.MessageAttributes.TryGetValue("SomeKey", out var attributeValue);

        logger.LogInformation("Received {MessageType} with message {Message}", nameof(SomeNativeMessage), eventMessage.ThisIsTheMessage);

        if (nativeAttributeFound)
        {
            logger.LogInformation("Found attribute 'SomeKey' with value '{AttributeValue}'", attributeValue.StringValue);
        }

        if (context.ReplyToAddress != null)
        {
            logger.LogInformation("Sending reply to '{ReplyToAddress}'", context.ReplyToAddress);

            await context.Reply(new SomeReply());
        }
    }
}
#endregion
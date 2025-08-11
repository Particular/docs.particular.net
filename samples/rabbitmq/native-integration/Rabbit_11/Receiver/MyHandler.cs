namespace Receiver;

using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;

public class MyHandler(ILogger<MyHandler> logger) : IHandleMessages<MyMessage>
{
    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        #region AccessToNativeMessageDetails
        var nativeAppId = context.Extensions.Get<BasicDeliverEventArgs>()?.BasicProperties.AppId;
        #endregion

        logger.LogInformation("Got `MyMessage` with id: {MessageId}, property value: {SomeProperty}, native application id: {NativeAppId}", context.MessageId, message.SomeProperty, nativeAppId);
        return Task.CompletedTask;
    }
}
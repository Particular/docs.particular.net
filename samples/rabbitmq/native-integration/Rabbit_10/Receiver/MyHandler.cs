namespace Receiver;

using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;

public class MyHandler(ILogger<MyHandler> logger) : IHandleMessages<MyMessage>
{
    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        #region AccessToNativeMessageDetails
        var nativeAppId = context.Extensions.Get<BasicDeliverEventArgs>().BasicProperties.AppId;
        #endregion

        logger.LogInformation($"Got `MyMessage` with id: {context.MessageId}, property value: {message.SomeProperty}, native application id: {nativeAppId}");
        return Task.CompletedTask;
    }
}
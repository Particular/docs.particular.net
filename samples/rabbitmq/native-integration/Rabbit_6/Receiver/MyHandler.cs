using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using RabbitMQ.Client.Events;

public class MyHandler :
    IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger<MyHandler>();

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        #region AccessToNativeMessageDetails

        var nativeAppId = context.Extensions.Get<BasicDeliverEventArgs>().BasicProperties.AppId;
        
        #endregion

        log.Info($"Got `MyMessage` with id: {context.MessageId}, property value: {message.SomeProperty}, native application id: {nativeAppId}");
        return Task.CompletedTask;
    }
}
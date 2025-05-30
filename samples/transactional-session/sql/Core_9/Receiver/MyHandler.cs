using NServiceBus.Logging;

#region txsession-handler
public class MyHandler : IHandleMessages<MyMessage>
{
    static readonly ILog log = LogManager.GetLogger<MyHandler>();

    public async Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        log.Info($"Message received at endpoint, message text - {message.MessageText}");

        await Task.CompletedTask;

    }
}
#endregion
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class MyMessageHandler :
    IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger<MyMessageHandler>();

    public async Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        #region RequestHandler

        log.Info($"Request {message.Id}");
        await context.Publish(new MyEvent
        {
            Id = message.Id
        }).ConfigureAwait(false);

        await context.Reply(new MyReply
        {
            Id = message.Id
        }).ConfigureAwait(false);

        #endregion
    }
}
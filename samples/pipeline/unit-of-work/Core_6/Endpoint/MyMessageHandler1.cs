using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region message-handlers
class MyMessageHandler1 : IHandleMessages<MyMessage>
{
    public async Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        await context.Store(new MyEntity());

        Logger.Info($"{nameof(MyMessageHandler1)}({context.MessageId}) got UOW instance {context.GetSession()}");
    }

    ILog Logger = LogManager.GetLogger<MyMessageHandler1>();
}
#endregion
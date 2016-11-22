using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class MyMessageHandler2 : IHandleMessages<MyMessage>
{
    public async Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        await context.Store(new MyOtherEntity());

        Logger.Info($"{nameof(MyMessageHandler2)}({context.MessageId}) got UOW instance {context.GetSession().GetHashCode()}");
    }

    ILog Logger = LogManager.GetLogger<MyMessageHandler1>();
}
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class MyMessageHandler2 :
    IHandleMessages<MyMessage>
{
    ILog log = LogManager.GetLogger<MyMessageHandler1>();

    public async Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        await context.Store(new MyOtherEntity());

        log.Info($"{nameof(MyMessageHandler2)}({context.MessageId}) got UOW instance {context.GetSession().GetHashCode()}");
    }
}
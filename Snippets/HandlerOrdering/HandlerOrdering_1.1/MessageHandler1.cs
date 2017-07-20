using System.Threading.Tasks;
using HandlerOrdering;
using NServiceBus;

#region express-order1
public class MessageHandler1 :
    IHandleMessages<MyMessage>,
    IWantToRunAfter<MessageHandler3>
{
    #endregion

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        return Task.CompletedTask;
    }
}
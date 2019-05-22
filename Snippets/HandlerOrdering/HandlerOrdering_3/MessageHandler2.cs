using System.Threading.Tasks;
using HandlerOrdering;
using NServiceBus;

#region express-order2
public class MessageHandler2 :
    IHandleMessages<MyMessage>,
    IWantToRunAfter<MessageHandler1>
{
    #endregion

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        return Task.CompletedTask;
    }
}
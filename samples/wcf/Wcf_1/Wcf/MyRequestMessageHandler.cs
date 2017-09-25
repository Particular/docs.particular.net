using System.Threading.Tasks;
using NServiceBus;

#region wcf-reply-handler

public class MyRequestMessageHandler :
    IHandleMessages<MyRequestMessage>
{
    public Task Handle(MyRequestMessage message, IMessageHandlerContext context)
    {
        if (message.Info == "Cancel")
        {
            return Task.CompletedTask;
        }
        var response = new MyResponseMessage
        {
            Info = message.Info
        };
        return context.Reply(response);
    }
}
#endregion
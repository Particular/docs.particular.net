using System.Threading.Tasks;
using NServiceBus;

#region SimpleHandler
public class MyReplyingHandler :
    IHandleMessages<MyRequest>
{
    public Task Handle(MyRequest message, IMessageHandlerContext context)
    {
        return context.Reply(new MyResponse());
    }
}
#endregion
using System.Threading.Tasks;
using NServiceBus;
#region OtherHandler
public class OtherHandler :
    IHandleMessages<MyMessage>
{
    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        return context.SendLocal(new OtherMessage());
    }
}
#endregion
using System.Threading.Tasks;
using NServiceBus;

namespace Core_9.EmptyHandler;

#region EmptyHandler
public class DoSomethingHandler :
    IHandleMessages<DoSomething>
{
    public Task Handle(DoSomething message, IMessageHandlerContext context)
    {
        // Do something with the message here
        return Task.CompletedTask;
    }
}
#endregion
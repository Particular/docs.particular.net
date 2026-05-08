namespace Core.EmptyHandler;

#region EmptyHandler
public class DoSomethingHandler : IHandleMessages<DoSomething>
{
    public Task Handle(DoSomething message, IMessageHandlerContext context)
    {
        // Do something with the message here
        return Task.CompletedTask;
    }
}
#endregion
namespace Core.EmptyHandlerAsync;

#region EmptyHandlerAsync
public class DoSomethingHandler : IHandleMessages<DoSomething>
{
    public async Task Handle(DoSomething message, IMessageHandlerContext context)
    {
        // Do something with the message here
    }
}
#endregion

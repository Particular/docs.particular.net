#region InjectingDependency
public class MyHandler(Greeter greeter) : IHandleMessages<MyMessage>
{
    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        greeter.SayHello();
        return Task.CompletedTask;
    }
}
#endregion

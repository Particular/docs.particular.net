using System.Threading.Tasks;
using NServiceBus;
#region InjectingDependency
public class MyHandler :
    IHandleMessages<MyMessage>
{
    private readonly Greeter greeter;

    public MyHandler(Greeter greeter) =>
        this.greeter = greeter;

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        greeter.SayHello();
        return Task.CompletedTask;
    }
}
#endregion

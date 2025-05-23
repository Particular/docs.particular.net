using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
#region InjectingDependency
public class MyHandler :
    IHandleMessages<MyMessage>
{
    private readonly Greeter greeter;
    private static readonly ILog log = LogManager.GetLogger<Greeter>();

    public MyHandler(Greeter greeter) =>
        this.greeter = greeter;

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        log.Info("Message received");
        greeter.SayHello();
        return Task.CompletedTask;
    }
}
#endregion

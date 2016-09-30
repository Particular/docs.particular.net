using NServiceBus;
using System.Threading.Tasks;

#region InjectingDependency
public class MyHandler :
    IHandleMessages<MyMessage>
{
    MyService myService;

    public MyHandler(MyService myService)
    {
        this.myService = myService;
    }

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        myService.WriteHello();
        return Task.CompletedTask;
    }
}
#endregion
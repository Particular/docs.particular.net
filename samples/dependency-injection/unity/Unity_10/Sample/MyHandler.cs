using System.Threading.Tasks;
using NServiceBus;
#region InjectingDependency
public class MyHandler :
    IHandleMessages<MyMessage>
{
    MyService myService;

    public MyHandler(MyService myService)
    {
        this.myService = myService;
    }

    public MyOtherService OtherService { get; set; }

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        myService.WriteHello();
        OtherService.WriteHello();
        return Task.CompletedTask;
    }
}
#endregion
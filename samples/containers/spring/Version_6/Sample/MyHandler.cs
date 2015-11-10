using System.Threading.Tasks;
using NServiceBus;

#region InjectingDependency
public class MyHandler : IHandleMessages<MyMessage>
{
    MyService myService;

    public MyHandler(MyService myService)
    {
        this.myService = myService;
    }

    public Task Handle(MyMessage message)
    {
        myService.WriteHello();
        return Task.FromResult(0);
    }
}
#endregion
using NServiceBus;

#region InjectingDependency
public class MyHandler : IHandleMessages<MyMessage>
{
    MyService myService;

    public MyHandler(MyService myService)
    {
        this.myService = myService;
    }

    public void Handle(MyMessage message)
    {
        myService.WriteHello();
    }
}
#endregion
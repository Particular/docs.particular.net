public class MyHandler(MyService myService) : IHandleMessages<MyMessage>
{
    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        myService.WriteHello();
        return Task.CompletedTask;
    }
}

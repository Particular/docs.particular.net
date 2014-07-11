using NServiceBus;

public class BusExtensionMethodForHandler
{
    // start code BusExtensionMethodForHandlerV4
    public class MyHandler : IHandleMessages<MyMessage>
    {
        public void Handle(MyMessage message)
        {
            this.Bus().Reply(new OtherMessage());
        }
    }
    // end code BusExtensionMethodForHandlerV4
    public class MyMessage
    {
    }
    public class OtherMessage
    {
    }
}
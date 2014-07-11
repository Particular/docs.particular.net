using NServiceBus;

public class BusExtensionMethodForHandlerReplacement
{
    // start code BusExtensionMethodForHandlerReplacementV5
    public class MyHandler : IHandleMessages<MyMessage>
    {
        IBus bus;

        public MyHandler(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(MyMessage message)
        {
            bus.Reply(new OtherMessage());
        }
    }
    // end code BusExtensionMethodForHandlerReplacementV5
    public class MyMessage
    {
    }
    public class OtherMessage
    {
    }
}
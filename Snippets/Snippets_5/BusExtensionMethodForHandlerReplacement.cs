using NServiceBus;

public class BusExtensionMethodForHandlerReplacement
{
    #region BusExtensionMethodForHandlerReplacementV5
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
    #endregion
    public class MyMessage
    {
    }
    public class OtherMessage
    {
    }
}
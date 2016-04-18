namespace Core4.Headers
{
    using NServiceBus;

    #region header-outgoing-handler
    public class WriteHandler : IHandleMessages<MyMessage>
    {
        IBus bus;

        public WriteHandler(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(MyMessage message)
        {
            SomeOtherMessage someOtherMessage = new SomeOtherMessage();
            bus.SetMessageHeader(someOtherMessage, "MyCustomHeader", "My custom value");
            bus.Send(someOtherMessage);
        }
    }
    #endregion
}

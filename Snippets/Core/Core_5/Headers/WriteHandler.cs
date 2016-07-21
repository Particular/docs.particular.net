namespace Core5.Headers
{
    using NServiceBus;

    #region header-outgoing-handler

    public class WriteHandler :
        IHandleMessages<MyMessage>
    {
        IBus bus;

        public WriteHandler(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(MyMessage message)
        {
            var someOtherMessage = new SomeOtherMessage();
            bus.SetMessageHeader(
                msg: someOtherMessage,
                key: "MyCustomHeader",
                value: "My custom value");
            bus.Send(someOtherMessage);
        }
    }

    #endregion
}
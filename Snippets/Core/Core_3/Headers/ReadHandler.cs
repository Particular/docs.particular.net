namespace Core3.Headers
{
    using NServiceBus;

    #region header-incoming-handler
    public class ReadHandler :
        IHandleMessages<MyMessage>
    {
        IBus bus;

        public ReadHandler(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(MyMessage message)
        {
            var headers = bus.CurrentMessageContext.Headers;
            var nsbVersion = headers[Headers.NServiceBusVersion];
            var customHeader = headers["MyCustomHeader"];
        }
    }
    #endregion
}

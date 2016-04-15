namespace Snippets4.Headers
{
    using System.Collections.Generic;
    using NServiceBus;

    #region header-incoming-handler
    public class ReadHandler : IHandleMessages<MyMessage>
    {
        IBus bus;

        public ReadHandler(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(MyMessage message)
        {
            IDictionary<string, string> headers = bus.CurrentMessageContext.Headers;
            string nsbVersion = headers[Headers.NServiceBusVersion];
            string customHeader = headers["MyCustomHeader"];
        }
    }
    #endregion
}

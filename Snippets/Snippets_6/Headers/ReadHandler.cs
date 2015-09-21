namespace Snippets6.Headers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NServiceBus;

    #region header-incoming-handler
    public class ReadHandler : IHandleMessages<MyMessage>
    {
        IBus bus;

        public ReadHandler(IBus bus)
        {
            this.bus = bus;
        }

        public Task Handle(MyMessage message)
        {
            IDictionary<string, string> headers = bus.CurrentMessageContext.Headers;
            string nsbVersion = headers[Headers.NServiceBusVersion];
            string customHeader = headers["MyCustomHeader"];
            return Task.FromResult(0);
        }
    }
    #endregion
}

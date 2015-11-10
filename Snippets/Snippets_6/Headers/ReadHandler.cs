namespace Snippets6.Headers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NServiceBus;

    #region header-incoming-handler
    public class ReadHandler : IHandleMessages<MyMessage>
    {
        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            IReadOnlyDictionary<string, string> headers = context.MessageHeaders;
            string nsbVersion = headers[Headers.NServiceBusVersion];
            string customHeader = headers["MyCustomHeader"];
        }
    }
    #endregion
}

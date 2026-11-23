namespace Wcf_3.Routing
{
    using System.Threading.Tasks;
    using NServiceBus;

    #region WcfReplyFromAnotherEndpoint

    public class Handler :
        IHandleMessages<Request>
    {
        public Task Handle(Request message, IMessageHandlerContext context)
        {
            // Requires NServiceBus.Callbacks
            return context.Reply(new Response());
        }
    }

    #endregion
}
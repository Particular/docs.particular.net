namespace Wcf_2.Int
{
    using System.Threading.Tasks;
    using NServiceBus;

    #region WcfIntCallbackResponse

    public class Handler :
        IHandleMessages<Message>
    {
        public Task Handle(Message message, IMessageHandlerContext context)
        {
            return context.Reply(10);
        }
    }

    #endregion
}
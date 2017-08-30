namespace Wcf_2.Enum
{
    using System.Threading.Tasks;
    using NServiceBus;

    #region WcfEnumCallbackResponse

    public class Handler :
        IHandleMessages<Message>
    {
        public Task Handle(Message message, IMessageHandlerContext context)
        {
            return context.Reply(Status.OK);
        }
    }

    #endregion
}
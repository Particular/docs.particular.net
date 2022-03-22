namespace Callbacks.Int
{
    using System.Threading.Tasks;
    using NServiceBus;

    #region IntCallbackResponse

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
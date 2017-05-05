namespace Callbacks.Enum
{
    using System.Threading.Tasks;
    using NServiceBus;

    #region EnumCallbackResponse

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
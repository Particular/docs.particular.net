namespace Core6.Callback.Int
{
    using System.Threading.Tasks;
    using NServiceBus;

    #region IntCallbackResponse

    public class Handler : IHandleMessages<Message>
    {
        public async Task Handle(Message message, IMessageHandlerContext context)
        {
            await context.Reply(10);
        }
    }

    #endregion
}


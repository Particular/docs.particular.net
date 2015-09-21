namespace Snippets5.Callback.Int
{
    using System.Threading.Tasks;
    using NServiceBus;

    #region IntCallbackResponse

    public class Handler : IHandleMessages<Message>
    {
        IBus bus;

        public Handler(IBus bus)
        {
            this.bus = bus;
        }

        public async Task Handle(Message message)
        {
            await bus.ReplyAsync(10);
        }
    }

    #endregion
}


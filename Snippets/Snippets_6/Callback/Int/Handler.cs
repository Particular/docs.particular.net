namespace Snippets5.Callback.Int
{
    using NServiceBus;

    #region IntCallbackResponse

    public class Handler : IHandleMessages<Message>
    {
        IBus bus;

        public Handler(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(Message message)
        {
            bus.Reply(10);
        }
    }

    #endregion
}


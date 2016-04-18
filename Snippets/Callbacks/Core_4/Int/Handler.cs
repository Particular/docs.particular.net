using NServiceBus;

namespace Core4.Callback.Int
{

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
            bus.Return(10);
        }
    }

    #endregion
}


using NServiceBus;

namespace Snippets4.Callback.Enum
{

    #region EnumCallbackResponse

    public class Handler : IHandleMessages<Message>
    {
        IBus bus;

        public Handler(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(Message message)
        {
            bus.Return(Status.OK);
        }
    }

    #endregion
}

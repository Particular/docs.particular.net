using NServiceBus;

namespace Core4.Callback.Object
{

    #region ObjectCallbackResponse

    public class Handler : IHandleMessages<Message>
    {
        IBus bus;

        public Handler(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(Message message)
        {
            bus.Reply(new ResponseMessage
            {
                Property = "PropertyValue"
            });
        }
    }

    #endregion
}


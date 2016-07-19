namespace Core5.Object
{
    using NServiceBus;

    #region ObjectCallbackResponse

    public class Handler :
        IHandleMessages<Message>
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


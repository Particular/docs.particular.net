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
            var response = new ResponseMessage
            {
                Property = "PropertyValue"
            };
            bus.Reply(response);
        }
    }

    #endregion
}


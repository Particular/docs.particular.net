namespace Core3.Object
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
            var responseMessage = new ResponseMessage
            {
                Property = "PropertyValue"
            };
            bus.Reply(responseMessage);
        }
    }

    #endregion
}
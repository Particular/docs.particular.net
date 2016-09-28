namespace Core3.Enum
{
    using NServiceBus;

    #region EnumCallbackResponse

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
            bus.Return(Status.OK);
        }
    }

    #endregion
}
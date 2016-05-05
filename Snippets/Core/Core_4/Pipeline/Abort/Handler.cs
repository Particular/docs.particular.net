namespace Core4.Pipeline.Abort
{
    using NServiceBus;

    #region AbortHandler

    class Handler : IHandleMessages<MyMessage>
    {
        IBus bus;

        public Handler(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(MyMessage message)
        {
            // may want to log a reason here
            bus.DoNotContinueDispatchingCurrentMessageToHandlers();
        }
    }

    #endregion
}

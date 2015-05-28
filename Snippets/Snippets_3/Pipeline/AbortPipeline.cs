namespace AbortPipeline
{
    using NServiceBus;

    #region AbortHandler

    class AbortHandler : IHandleMessages<MyMessage>
    {
        IBus bus;

        public AbortHandler(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(MyMessage message)
        {
            // you may also want to log a reason here
            bus.DoNotContinueDispatchingCurrentMessageToHandlers();
        }
    }

    #endregion
    public class MyMessage
    {

    }
}
namespace Core5.Sagas
{
    using NServiceBus;
    using NServiceBus.Saga;

    #region saga-not-found

    public class SagaNotFoundHandler : IHandleSagaNotFound
    {
        IBus bus;

        public SagaNotFoundHandler(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(object message)
        {
            bus.Reply(new SagaDisappearedMessage());
        }
    }

    public class SagaDisappearedMessage
    {
    }

    #endregion
}
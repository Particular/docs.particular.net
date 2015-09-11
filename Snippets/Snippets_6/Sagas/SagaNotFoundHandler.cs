namespace Snippets6.Sagas
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Sagas;

    #region saga-not-found

    public class SagaNotFoundHandler : IHandleSagaNotFound
    {
        IBus bus;

        public SagaNotFoundHandler(IBus bus)
        {
            this.bus = bus;
        }

        public Task Handle(object message)
        {
            bus.Reply(new SagaDisappearedMessage());
            return Task.FromResult(0);
        }
    }

    public class SagaDisappearedMessage
    {
    }

    #endregion
}
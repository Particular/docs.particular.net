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

        public async Task Handle(object message)
        {
            await bus.ReplyAsync(new SagaDisappearedMessage());
        }
    }

    public class SagaDisappearedMessage
    {
    }

    #endregion
}
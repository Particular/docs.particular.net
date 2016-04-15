namespace Core6.Sagas
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Sagas;

    #region saga-not-found

    public class SagaNotFoundHandler : IHandleSagaNotFound
    {
        public async Task Handle(object message, IMessageProcessingContext context)
        {
            await context.Reply(new SagaDisappearedMessage());
        }
    }

    public class SagaDisappearedMessage
    {
    }

    #endregion
}
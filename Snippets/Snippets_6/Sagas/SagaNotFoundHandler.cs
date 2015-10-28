namespace Snippets6.Sagas
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Sagas;

    #region saga-not-found

    public class SagaNotFoundHandler : IHandleSagaNotFound
    {
        public async Task Handle(object message, IMessageProcessingContext context)
        {
            await context.ReplyAsync(new SagaDisappearedMessage());
        }
    }

    public class SagaDisappearedMessage
    {
    }

    #endregion
}
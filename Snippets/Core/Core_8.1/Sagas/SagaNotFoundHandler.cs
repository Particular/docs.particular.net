namespace Core8.Sagas
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Sagas;

    #region saga-not-found

    public class SagaNotFoundHandler :
        IHandleSagaNotFound
    {
        public Task Handle(object message, IMessageProcessingContext context)
        {
            var sagaDisappearedMessage = new SagaDisappearedMessage();
            return context.Reply(sagaDisappearedMessage);
        }
    }

    public class SagaDisappearedMessage
    {
    }

    #endregion
}
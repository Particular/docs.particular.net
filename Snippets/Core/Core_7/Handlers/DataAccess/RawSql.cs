namespace Core7.Handlers.DataAccess
{
    using System.Threading.Tasks;
    using NServiceBus;

    public class SubmitOrderHandler : IHandleMessages<SubmitOrder>
    {
        public Task Handle(SubmitOrder message, IMessageHandlerContext context)
        {
            var session = context.SynchronizedStorageSession.SqlPersistenceSession();

        }
    }

    public class SubmitOrder
    {

    }
}

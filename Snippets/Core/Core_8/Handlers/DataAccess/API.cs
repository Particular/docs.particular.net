namespace Core8.Handlers.DataAccess
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Persistence;

    public class API : IHandleMessages<MyMessage>
    {
        #region BusinessData-SynchronizedStorageSession

        public Task Handle(MyMessage message, 
            IMessageHandlerContext context)
        {
            var session = context.SynchronizedStorageSession
                .MyPersistenceSession();

            //Business logic

            return Task.CompletedTask;
        }

        #endregion
    }

    public static class MyOrmExtensions
    {
        public static object MyPersistenceSession(this SynchronizedStorageSession s)
        {
            throw new NotImplementedException();
        }
    }
}

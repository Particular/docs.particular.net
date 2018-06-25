namespace Core7.Handlers.DataAccess
{
    using System;
    using NServiceBus.Persistence;

    public static class MyOrmExtensions
    {
        public static IMyOrmSession MyOrmSession(this SynchronizedStorageSession s)
        {
            throw new NotImplementedException();
        }
        
        public static void MarkAsProcessed(this Managed.IdempotencyEnforcer o, string messageId, Order order)
        {
        }
    }
}
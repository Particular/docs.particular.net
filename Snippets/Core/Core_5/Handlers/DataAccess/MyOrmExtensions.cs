namespace Core5.Handlers
{
    using System;
    using NServiceBus;

    public static class MyOrmExtensions
    {
        public static IMyOrmSession MyOrmSession(this IBus b)
        {
            throw new NotImplementedException();
        }

        public static void MarkAsProcessed(this Managed.IdempotencyEnforcer o, string messageId, Order order)
        {
        }
    }
}
namespace NServiceBus.MyPersistence;

using NServiceBus;
using NServiceBus.TransactionalSession;

static class MyPersistenceConfigExtensions
{
    public static void EnableTransactionalSession(this PersistenceExtensions<MyPersistence> persistence, TransactionalSessionOptions transactionalSessionOptions = null)
    {
    }
}
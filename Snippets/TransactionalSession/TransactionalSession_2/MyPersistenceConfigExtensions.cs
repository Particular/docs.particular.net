namespace NServiceBus.MyPersistence
{
    using NServiceBus;

    static class MyPersistenceConfigExtensions
    {
        public static void EnableTransactionalSession(this PersistenceExtensions<MyPersistence> persistence)
        {
        }
    }
}
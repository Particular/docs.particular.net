namespace Core6.Sagas.FindSagas
{
    using System;
    using NServiceBus.Persistence;

    public static class SynchronizedStorageSessionEx
    {
        public static dynamic GetDbSession(this SynchronizedStorageSession session)
        {
            throw new NotImplementedException();
        }
    }
}
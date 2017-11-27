using System;
using NServiceBus.Persistence;

class EntityFrameworkUnitOfWork
{
    Func<SynchronizedStorageSession, ReceiverDataContext> contextFactory;
    ReceiverDataContext context;

    public EntityFrameworkUnitOfWork(Func<SynchronizedStorageSession, ReceiverDataContext> contextFactory)
    {
        this.contextFactory = contextFactory;
    }

    public ReceiverDataContext GetDataContext(SynchronizedStorageSession storageSession)
    {
        if (context == null)
        {
            context = contextFactory(storageSession);

            
        }
        return context;
    }
}

using NServiceBus;
using NServiceBus.Persistence;

#region UnitOfWork

class EntityFrameworkUnitOfWork
{
    ReceiverDataContext context;

    public ReceiverDataContext GetDataContext(SynchronizedStorageSession storageSession)
    {
        if (context == null)
        {
            var dbConnection = storageSession.Session().Connection;
            context = new ReceiverDataContext(dbConnection);

            //Don't use transaction because connection is enlisted in the TransactionScope
            context.Database.UseTransaction(null);

            //Call SaveChanges before completing storage session
            storageSession.OnSaveChanges(x => context.SaveChangesAsync());
        }
        return context;
    }
}

#endregion
using NServiceBus;
using NServiceBus.Pipeline;

#region UnitOfWork

class EntityFrameworkUnitOfWork
{
    public ReceiverDataContext Context { get; private set; }

    public void Initialize(IInvokeHandlerContext context)
    {
        if (Context != null)
        {
            //Only the first handler in transaction initializes the Unit of Work
            return;
        }
        var storageSession = context.SynchronizedStorageSession;

        var dbConnection = storageSession.Session().Connection;
        Context = new ReceiverDataContext(dbConnection);

        //Don't use transaction because connection is enlisted in the TransactionScope
        Context.Database.UseTransaction(null);

        //Call SaveChanges before completing storage session
        storageSession.OnSaveChanges(x => Context.SaveChangesAsync());
    }
}

#endregion
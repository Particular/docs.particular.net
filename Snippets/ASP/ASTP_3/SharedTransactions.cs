using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using NServiceBus;
using NServiceBus.Persistence.AzureTable;

class MyMessage
{
    public string ItemId { get; set; }
}

class UsageHandler : IHandleMessages<MyMessage>
{
    ToDoActivity test1 = new ToDoActivity();
    ToDoActivity test2 = new ToDoActivity();
    ToDoActivity test3 = new ToDoActivity();
    ToDoActivity test4 = new ToDoActivity();

    #region HandlerSharedTransaction
    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        //setup the items for the batch...

        var session = context.SynchronizedStorageSession.AzureTablePersistenceSession();

        session.Batch.Add(TableOperation.Insert(test1));
        session.Batch.Add(TableOperation.Replace(test2));
        session.Batch.Add(TableOperation.InsertOrReplace(test3));
        session.Batch.Add(TableOperation.Delete(test4));

        return Task.CompletedTask;
    }
    #endregion
}

#region TransactionalBatchRegisteredWithDependencyInjectionResolvedInHandler

public class ToDoActivity : TableEntity
{
    public string Description { get; set; }
}

class MyHandler : IHandleMessages<MyMessage>
{
    public MyHandler(IAzureTableStorageSession storageSession)
    {
        transactionalBatch = storageSession.Batch;
    }

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        var entity = new ToDoActivity
        {
            PartitionKey = "PartitionKey",
            RowKey = "RowKey"
        };

        transactionalBatch.Add(TableOperation.Insert(entity));

        return Task.CompletedTask;
    }

    private readonly TableBatchOperation transactionalBatch;
}

#endregion

#region TransactionalBatchRegisteredWithDependencyInjectionResolvedInCustomType

class MyCustomDependency
{
    public MyCustomDependency(IAzureTableStorageSession storageSession)
    {
        transactionalBatch = storageSession.Batch;
    }

    public void DeleteInAzureTable(string itemId)
    {
        var entity = new ToDoActivity
        {
            PartitionKey = "PartitionKey",
            RowKey = "RowKey"
        };

        transactionalBatch.Add(TableOperation.Insert(entity));
    }

    private readonly TableBatchOperation transactionalBatch;
}

class MyHandlerWithCustomDependency : IHandleMessages<MyMessage>
{
    public MyHandlerWithCustomDependency(MyCustomDependency customDependency)
    {
        this.customDependency = customDependency;
    }

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        customDependency.DeleteInAzureTable(message.ItemId);

        return Task.CompletedTask;
    }

    private readonly MyCustomDependency customDependency;
}

#endregion
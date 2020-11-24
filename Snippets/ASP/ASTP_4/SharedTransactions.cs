using System;
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
    #region HandlerSharedTransaction
    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        var session = context.SynchronizedStorageSession.AzureTablePersistenceSession();

        var test1 = new ToDoActivity { PartitionKey = session.PartitionKey, RowKey = Guid.NewGuid().ToString() };
        var test2 = new ToDoActivity { PartitionKey = session.PartitionKey, RowKey = Guid.NewGuid().ToString() };
        var test3 = new ToDoActivity { PartitionKey = session.PartitionKey, RowKey = Guid.NewGuid().ToString() };
        var test4 = new ToDoActivity { PartitionKey = session.PartitionKey, RowKey = Guid.NewGuid().ToString() };

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
        this.storageSession = storageSession;
    }

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        var entity = new ToDoActivity
        {
            PartitionKey = storageSession.PartitionKey,
            RowKey = "RowKey"
        };

        storageSession.Batch.Add(TableOperation.Insert(entity));

        return Task.CompletedTask;
    }

    private readonly IAzureTableStorageSession storageSession;
}

#endregion

#region TransactionalBatchRegisteredWithDependencyInjectionResolvedInCustomType

class MyCustomDependency
{
    public MyCustomDependency(IAzureTableStorageSession storageSession)
    {
        this.storageSession = storageSession;
    }

    public void DeleteInAzureTable(string itemId)
    {
        var entity = new ToDoActivity
        {
            PartitionKey = storageSession.PartitionKey,
            RowKey = itemId
        };

        storageSession.Batch.Add(TableOperation.Insert(entity));
    }

    private readonly IAzureTableStorageSession storageSession;
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
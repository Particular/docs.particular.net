using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using NServiceBus;

class MyMessage
{
    public string ItemId { get; set; }
}

class UsageHandler : IHandleMessages<MyMessage>
{
    ToDoActivity test1 = new ToDoActivity();
    ToDoActivity test2 = new ToDoActivity();
    ToDoActivity test3 = new ToDoActivity();

    public class ToDoActivity
    {
        public string id { get; set; }
    }

    #region CosmosDBHandlerSharedTransaction
    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        //setup the items for the batch...

        var session = context.SynchronizedStorageSession.CosmosPersistenceSession();

        session.Batch
                .CreateItem(test1)
                .ReplaceItem(test2.id, test2)
                .UpsertItem(test3)
                .DeleteItem("/item/id");

        return Task.CompletedTask;
    }
    #endregion
}

#region CosmosDB-TransactionalBatchRegisteredWithDependencyInjectionResolvedInHandler

class MyHandler(ICosmosStorageSession storageSession) : IHandleMessages<MyMessage>
{
    readonly TransactionalBatch transactionalBatch = storageSession.Batch;

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        transactionalBatch.DeleteItem(message.ItemId);

        return Task.CompletedTask;
    }
}

#endregion

#region CosmosDB-TransactionalBatchRegisteredWithDependencyInjectionResolvedInCustomType

class MyCustomDependency(ICosmosStorageSession storageSession)
{
    readonly TransactionalBatch transactionalBatch = storageSession.Batch;

    public void DeleteItemInCosmos(string itemId)
    {
        transactionalBatch.DeleteItem(itemId);
    }
}

class MyHandlerWithCustomDependency(MyCustomDependency customDependency) : IHandleMessages<MyMessage>
{
    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        customDependency.DeleteItemInCosmos(message.ItemId);

        return Task.CompletedTask;
    }
}

#endregion
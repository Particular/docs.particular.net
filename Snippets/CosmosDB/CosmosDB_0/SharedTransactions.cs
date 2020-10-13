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

#region TransactionalBatchRegisteredWithDependencyInjectionResolvedInHandler

class MyHandler : IHandleMessages<MyMessage>
{
    public MyHandler(ICosmosStorageSession storageSession)
    {
        transactionalBatch = storageSession.Batch;
    }

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        transactionalBatch.DeleteItem(message.ItemId);

        return Task.CompletedTask;
    }

    private readonly TransactionalBatch transactionalBatch;
}

#endregion

#region TransactionalBatchRegisteredWithDependencyInjectionResolvedInCustomType

class MyCustomDependency
{
    private readonly TransactionalBatch transactionalBatch;

    public MyCustomDependency(ICosmosStorageSession storageSession)
    {
        transactionalBatch = storageSession.Batch;
    }

    public void DeleteItemInCosmos(string itemId)
    {
        transactionalBatch.DeleteItem(itemId);
    }
}

class MyHandlerWithCustomDependency : IHandleMessages<MyMessage>
{
    public MyHandlerWithCustomDependency(MyCustomDependency customDependency)
    {
        this.customDependency = customDependency;
    }

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        customDependency.DeleteItemInCosmos(message.ItemId);

        return Task.CompletedTask;
    }

    private readonly MyCustomDependency customDependency;
}

#endregion
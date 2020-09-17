using System.Threading.Tasks;
using NServiceBus;

class MyMessage { }

class MyMessageHandler : IHandleMessages<MyMessage>
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

        context.SynchronizedStorageSession.GetSharedTransactionalBatch()
                .CreateItem<ToDoActivity>(test1)
                .ReplaceItem<ToDoActivity>(test2.id, test2)
                .UpsertItem<ToDoActivity>(test3)
                .DeleteItem("/item/id");

        return Task.CompletedTask;
    }
    #endregion
}

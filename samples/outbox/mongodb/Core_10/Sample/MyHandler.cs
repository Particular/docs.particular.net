using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using NServiceBus;

public class MyHandler : IHandleMessages<MyMessage>
{
    #region Handler
    public async Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        Console.WriteLine($"Processing MessageId {context.MessageId}");

        var mongoPersistenceSession = context.SynchronizedStorageSession.MongoPersistenceSession();

        await mongoPersistenceSession.MongoSession!.Client.GetDatabase("Samples_Outbox_Demo")
            .GetCollection<MyDocument>("businessobjects")
            .InsertOneAsync(mongoPersistenceSession.MongoSession, new MyDocument
            {
                MessageId = context.MessageId,
            }, cancellationToken: context.CancellationToken);
    }
    #endregion
}

public class MyDocument
{
    public ObjectId Id { get; set; }
    public string MessageId { get; set; }
}
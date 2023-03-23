using Amazon.DynamoDBv2.Model;
using NServiceBus;

namespace DynamoDB_1;

class SynchronizedSession : IHandleMessages<MyMessage>
{
#pragma warning disable CS1998
#region DynamoDBSynchronizedSession
    public async Task Handle(MyMessage message, IMessageHandlerContext context)

    {
        //...

        var dynamoSession = context.SynchronizedStorageSession.DynamoDBPersistenceSession();
        dynamoSession.Add(new TransactWriteItem
        {
            // add database operations here
        });

        //...

    }
#endregion
#pragma warning restore CS1998
}

class MyMessage
{
}
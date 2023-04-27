using Amazon.DynamoDBv2.Model;
using NServiceBus;
using NServiceBus.Testing;
using NUnit.Framework;

namespace DynamoDB_1;

class MyMessageHandler : IHandleMessages<MyMessage>
{
#pragma warning disable CS1998
#region DynamoDBSynchronizedSession
    public async Task Handle(MyMessage message, IMessageHandlerContext context)

    {
        //...

        var dynamoSession = context.SynchronizedStorageSession.DynamoPersistenceSession();
        dynamoSession.Add(new TransactWriteItem
        {
            // add database operations here
        });

        //...

    }
#endregion
#pragma warning restore CS1998
}

class SynchronizedSessionTest
{
    #region DynamoDBTestingSessionUsage
    [Test]
    public async Task UnitTest()
    {
        var testableSession = new TestableDynamoSynchronizedStorageSession();
        var testableContext = new TestableMessageHandlerContext
        {
            SynchronizedStorageSession = testableSession
        };

        var handler = new MyMessageHandler();
        await handler.Handle(new MyMessage(), testableContext);

        // assert on transaction items:
        Assert.That(testableSession.TransactWriteItems, Has.Count.EqualTo(1));
    }
    #endregion
}

class MyMessage
{
}
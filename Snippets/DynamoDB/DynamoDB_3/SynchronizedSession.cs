using System.Text.Json.Serialization;

using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;

using NServiceBus;
using NServiceBus.Persistence.DynamoDB;
using NServiceBus.Testing;

namespace DynamoDB_3;

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

class MapperUsageWithoutKeyMapping
{
    #region DynamoDBMapperUsageWithoutKeyMappingCustomType
    class Customer
    {
        public string CustomerId { get; set; }

        public bool CustomerPreferred { get; set; }
    }
    #endregion

    class Message
    {
        public string CustomerId { get; set; }
    }
    public async Task UseWithoutKeyMapping(IDynamoStorageSession dynamoSession, IAmazonDynamoDB client, IMessageHandlerContext context)
    {
        var message = new Message();
        #region DynamoDBMapperUsageWithoutKeyMapping

        var getCustomer = new GetItemRequest
        {
            ConsistentRead = true,
            Key = new Dictionary<string, AttributeValue>
            {
                { "PK", new AttributeValue { S = $"CUSTOMERS#{message.CustomerId}" } },
                { "SK", new AttributeValue { S = message.CustomerId } }
            },
            TableName = "someTable"
        };

        var getCustomerResponse = await client.GetItemAsync(getCustomer, context.CancellationToken);

        var customer = Mapper.ToObject<Customer>(getCustomerResponse.Item);
        customer.CustomerPreferred = true;

        var customerMap = Mapper.ToMap(customer);
        // when PK and SK are not defined on the custom type they need to be added again
        customerMap["PK"] = getCustomerResponse.Item["PK"];
        customerMap["SK"] = getCustomerResponse.Item["SK"];

        dynamoSession.Add(new TransactWriteItem
        {
            Put = new Put
            {
                Item = customerMap,
                TableName = "someTable"
            }
        });
        #endregion
    }
}

class MapperUsageWithKeyMapping
{
    #region DynamoDBMapperUsageWithKeyMappingCustomType
    class Customer
    {
        [JsonPropertyName("PK")]
        public string PartitionKey { get; set; }

        [JsonPropertyName("SK")]
        public string SortKey { get; set; }

        public string CustomerId { get; set; }

        public bool CustomerPreferred { get; set; }
    }
    #endregion

    class Message
    {
        public string CustomerId { get; set; }
    }
    public async Task UseWithKeyMapping(IDynamoStorageSession dynamoSession, IAmazonDynamoDB client, IMessageHandlerContext context)
    {
        var message = new Message();

        var getCustomer = new GetItemRequest
        {
            ConsistentRead = true,
            Key = new Dictionary<string, AttributeValue>
            {
                { "PK", new AttributeValue { S = $"CUSTOMERS#{message.CustomerId}" } },
                { "SK", new AttributeValue { S = message.CustomerId } }
            },
            TableName = "someTable"
        };

        var getCustomerResponse = await client.GetItemAsync(getCustomer, context.CancellationToken);

        #region DynamoDBMapperUsageWithKeyMapping

        var customer = Mapper.ToObject<Customer>(getCustomerResponse.Item);
        customer.CustomerPreferred = true;

        dynamoSession.Add(new TransactWriteItem
        {
            Put = new Put
            {
                Item = Mapper.ToMap(customer),
                TableName = "someTable"
            }
        });
        #endregion
    }

}

class DynamoDBContextUsage
{
    #region DynamoDBMapperContextUsageCustomType
    [DynamoDBTable("customers")]
    class Customer
    {
        [DynamoDBHashKey("PK")]
        [JsonPropertyName("PK")]
        public string CustomerId { get; set; }

        [DynamoDBProperty("customer_preferred")]
        [JsonPropertyName("customer_preferred")]
        public bool CustomerPreferred { get; set; }
    }
    #endregion

    class Message
    {
        public string CustomerId { get; set; }
    }

    public async Task Use(IDynamoStorageSession dynamoSession, IDynamoDBContext dynamoDbContext, IMessageHandlerContext context)
    {
        var message = new Message();

        #region DynamoDBMapperContextUsage
        var customer = await dynamoDbContext.LoadAsync<Customer>(message.CustomerId, context.CancellationToken);

        customer.CustomerPreferred = true;

        // DO NOT call SaveAsync to participate in the synchronizes storage transaction
        // await dynamoDbContext.SaveAsync(customer, context.CancellationToken);

        dynamoSession.Add(new TransactWriteItem
        {
            Put = new Put
            {
                Item = Mapper.ToMap(customer),
                TableName = "someTable"
            }
        });

        #endregion
    }
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
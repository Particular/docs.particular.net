namespace Operations.RabbitMQ.Tests
{
    using NUnit.Framework;

    [TestFixture]
    [Explicit]
    public class QueueCreationTests
    {
        [Test]
        public void Foo()
        {
            QueueCreation.CreateAllForEndpoint(
                uri: "amqp://guest:guest@localhost:5672", 
                endpointName: "myendpoint2",
                durableMessages: true,
                createExchanges:true);

            QueueCreation.CreateLocalQueue(
                uri: "amqp://guest:guest@localhost:5672", 
                queueName: "error",
                durableMessages: true,
                createExchange: true);

            QueueCreation.CreateLocalQueue(
                uri: "amqp://guest:guest@localhost:5672", 
                queueName: "audit",
                durableMessages: true,
                createExchange: true);

        }
    }

}
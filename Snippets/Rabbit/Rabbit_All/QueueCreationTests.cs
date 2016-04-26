namespace Rabbit_All
{
    using NUnit.Framework;

    [TestFixture]
    [Explicit]
    public class QueueCreationTests
    {
        [Test]
        public void CreateQueuesForEndpoint()
        {

            QueueCreation.CreateQueuesForEndpoint(
                uri: "amqp://guest:guest@localhost:5672",
                endpointName: "myendpoint",
                durableMessages: true,
                createExchanges: true);

            QueueCreation.CreateQueue(
                uri: "amqp://guest:guest@localhost:5672",
                queueName: "error",
                durableMessages: true,
                createExchange: true);

            QueueCreation.CreateQueue(
                uri: "amqp://guest:guest@localhost:5672",
                queueName: "audit",
                durableMessages: true,
                createExchange: true);
        }

    }

}
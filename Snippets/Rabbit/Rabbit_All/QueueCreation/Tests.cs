using NUnit.Framework;

namespace Rabbit_All.QueueCreation
{
    [TestFixture]
    [Explicit]
    public class Tests
    {
        [Test]
        public void CreateQueuesForEndpoint()
        {

            CreateEndpointQueues.CreateQueuesForEndpoint(
                uri: "amqp://guest:guest@localhost:5672",
                endpointName: "myendpoint",
                durableMessages: true,
                createExchanges: true);

            QueueCreationUtils.CreateQueue(
                uri: "amqp://guest:guest@localhost:5672",
                queueName: "error",
                durableMessages: true,
                createExchange: true);

            QueueCreationUtils.CreateQueue(
                uri: "amqp://guest:guest@localhost:5672",
                queueName: "audit",
                durableMessages: true,
                createExchange: true);
        }

    }
}
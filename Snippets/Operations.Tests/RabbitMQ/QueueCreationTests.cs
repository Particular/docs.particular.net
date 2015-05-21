namespace Operations.RabbitMQ.Tests
{
    using NUnit.Framework;

    [TestFixture]
    [Explicit]
    public class QueueCreationTests
    {
        [Test]
        public void CreateQueuesForEndpoint()
        {
            #region rabbit-create-queues-endpoint-usage

            QueueCreation.CreateQueuesForEndpoint(
                uri: "amqp://guest:guest@localhost:5672",
                endpointName: "myendpoint",
                durableMessages: true,
                createExchanges: true);

            #endregion

            #region rabbit-create-queues-shared-usage

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

            #endregion
        }

        [Test]
        public void DeleteQueuesForEndpoint()
        {
            #region rabbit-delete-queues-endpoint-usage

            QueueCreation.DeleteQueuesForEndpoint(
                uri: "amqp://guest:guest@localhost:5672",
                endpointName: "myendpoint");

            #endregion

            #region rabbit-delete-queues-shared-usage

            QueueCreation.DeleteQueue(
                uri: "amqp://guest:guest@localhost:5672",
                queueName: "error");
            QueueCreation.DeleteQueue(
                uri: "amqp://guest:guest@localhost:5672",
                queueName: "audit");

            #endregion
        }
    }

}
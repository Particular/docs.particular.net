using NUnit.Framework;

namespace Rabbit_All.QueueDeletion
{
    [TestFixture]
    [Explicit]
    public class Tests
    {
        [Test]
        public void DeleteQueuesForEndpoint()
        {

            DeleteEndpointQueues.DeleteQueuesForEndpoint(
                uri: "amqp://guest:guest@localhost:5672",
                endpointName: "myendpoint");


            QueueDeletionUtils.DeleteQueue(
                uri: "amqp://guest:guest@localhost:5672",
                queueName: "error");
            QueueDeletionUtils.DeleteQueue(
                uri: "amqp://guest:guest@localhost:5672",
                queueName: "audit");

        }

    }
}
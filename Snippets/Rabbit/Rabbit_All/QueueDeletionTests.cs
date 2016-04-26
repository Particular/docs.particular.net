namespace Rabbit_All
{
    using NUnit.Framework;

    [TestFixture]
    [Explicit]
    public class QueueDeletionTests
    {
        [Test]
        public void DeleteQueuesForEndpoint()
        {

            QueueDeletion.DeleteQueuesForEndpoint(
                uri: "amqp://guest:guest@localhost:5672",
                endpointName: "myendpoint");


            QueueDeletion.DeleteQueue(
                uri: "amqp://guest:guest@localhost:5672",
                queueName: "error");
            QueueDeletion.DeleteQueue(
                uri: "amqp://guest:guest@localhost:5672",
                queueName: "audit");

        }

    }

}
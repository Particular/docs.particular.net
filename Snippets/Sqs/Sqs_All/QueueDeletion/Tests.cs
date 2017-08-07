namespace SqsAll.QueueDeletion
{
    using System;
    using System.Threading.Tasks;
    using SqsAll.QueueCreation;
    using NUnit.Framework;

    [TestFixture]
    [Explicit]
    public class Tests
    {

        [Test]
        public void DeleteAllQueues()
        {
            QueueDeletionUtils.DeleteAllQueues().GetAwaiter().GetResult();
        }

        [Test]
        public async Task DeleteQueuesForEndpoint()
        {
            await CreateEndpointQueues.CreateQueuesForEndpoint(
                endpointName: "myendpoint",
                maxTimeToLive: TimeSpan.FromSeconds(60))
                .ConfigureAwait(false);
            await DeleteEndpointQueues.DeleteQueuesForEndpoint("myendpoint")
                .ConfigureAwait(false);
            Assert.IsFalse(await QueueExistenceUtils.Exists("myendpoint"));

            await QueueCreationUtils.CreateQueue(
                queueName: "myerror",
                maxTimeToLive: TimeSpan.FromSeconds(60));
            await QueueDeletionUtils.DeleteQueue(queueName: "myerror")
                .ConfigureAwait(false);
            Assert.IsFalse(await QueueExistenceUtils.Exists("myerror"));

            await QueueCreationUtils.CreateQueue(
                queueName: "myaudit",
                maxTimeToLive: TimeSpan.FromSeconds(60))
                .ConfigureAwait(false);
            await QueueDeletionUtils.DeleteQueue(queueName: "myaudit")
                .ConfigureAwait(false);
            Assert.IsFalse(await QueueExistenceUtils.Exists("myaudit"));
        }
    }

}
namespace SqsAll.QueueCreation
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Amazon.SQS;
    using NUnit.Framework;
    using SqsAll.QueueDeletion;

    [TestFixture]
    [Explicit]
    public class Tests
    {

        [SetUp]
        [TearDown]
        public void Setup()
        {
            DeleteEndpointQueues.DeleteQueuesForEndpoint("myendpoint").GetAwaiter().GetResult();
            DeleteEndpointQueues.DeleteQueuesForEndpoint("mydefaultendpoint").GetAwaiter().GetResult();
            QueueDeletionUtils.DeleteQueue("myerror").GetAwaiter().GetResult();
            QueueDeletionUtils.DeleteQueue("myaudit").GetAwaiter().GetResult();
            QueueDeletionUtils.DeleteQueue("mydefaulterror").GetAwaiter().GetResult();
            QueueDeletionUtils.DeleteQueue("mydefaultaudit").GetAwaiter().GetResult();
        }

        [Test]
        public async Task CreateQueuesForEndpointWithOverridenMaxTTL()
        {
            var dayInSeconds = 86400;

            await CreateEndpointQueues.CreateQueuesForEndpoint(
                endpointName: "myendpoint",
                maxTimeToLive: TimeSpan.FromDays(1))
                .ConfigureAwait(false);

            Assert.IsTrue(await QueueExistenceUtils.Exists("myendpoint"));
            Assert.AreEqual(dayInSeconds, (await QueueAccessUtils.Exists("myendpoint", new List<string> { QueueAttributeName.MessageRetentionPeriod })).MessageRetentionPeriod);

            await QueueCreationUtils.CreateQueue(
                queueName: "myerror",
                maxTimeToLive: TimeSpan.FromDays(1))
                .ConfigureAwait(false);

            Assert.IsTrue(await QueueExistenceUtils.Exists("myerror"));
            Assert.AreEqual(dayInSeconds, (await QueueAccessUtils.Exists("myerror", new List<string> { QueueAttributeName.MessageRetentionPeriod })).MessageRetentionPeriod);

            await QueueCreationUtils.CreateQueue(
                queueName: "myaudit",
                maxTimeToLive: TimeSpan.FromDays(1))
                .ConfigureAwait(false);

            Assert.IsTrue(await QueueExistenceUtils.Exists("myaudit"));
            Assert.AreEqual(dayInSeconds, (await QueueAccessUtils.Exists("myaudit", new List<string> { QueueAttributeName.MessageRetentionPeriod })).MessageRetentionPeriod);
        }

        [Test]
        public async Task CreateQueuesForEndpointDefaultMaxTTL()
        {
            var FourDaysInSeconds = 4*86400;

            await CreateEndpointQueues.CreateQueuesForEndpoint("mydefaultendpoint")
                .ConfigureAwait(false);

            Assert.IsTrue(await QueueExistenceUtils.Exists("mydefaultendpoint"));
            Assert.AreEqual(FourDaysInSeconds, (await QueueAccessUtils.Exists("mydefaultendpoint", new List<string> { QueueAttributeName.MessageRetentionPeriod })).MessageRetentionPeriod);

            await QueueCreationUtils.CreateQueue("mydefaulterror")
                .ConfigureAwait(false);

            Assert.IsTrue(await QueueExistenceUtils.Exists("mydefaulterror"));
            Assert.AreEqual(FourDaysInSeconds, (await QueueAccessUtils.Exists("mydefaulterror", new List<string> { QueueAttributeName.MessageRetentionPeriod })).MessageRetentionPeriod);

            await QueueCreationUtils.CreateQueue(
                queueName: "mydefaultaudit")
                .ConfigureAwait(false);

            Assert.IsTrue(await QueueExistenceUtils.Exists("mydefaultaudit"));
            Assert.AreEqual(FourDaysInSeconds, (await QueueAccessUtils.Exists("mydefaultaudit", new List<string> { QueueAttributeName.MessageRetentionPeriod })).MessageRetentionPeriod);
        }
    }
}
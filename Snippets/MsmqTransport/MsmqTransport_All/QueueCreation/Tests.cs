namespace CoreAll.Msmq.QueueCreation
{
    using System;
    using System.Messaging;
    using CoreAll.Msmq.QueueDeletion;
    using NUnit.Framework;

    [TestFixture]
    [Explicit]
    public class Tests
    {

        [SetUp]
        [TearDown]
        public void Setup()
        {
            DeleteEndpointQueues.DeleteQueuesForEndpoint("myendpoint");
            QueueDeletionUtils.DeleteQueue("myerror");
            QueueDeletionUtils.DeleteQueue("myaudit");
        }

        [Test]
        public void CreateQueuesForEndpoint()
        {
            CreateEndpointQueues.CreateQueuesForEndpoint(
                endpointName: "myendpoint",
                account: Environment.UserName);

            Assert.IsTrue(MessageQueue.Exists(@".\private$\myendpoint"));

            QueueCreationUtils.CreateQueue(
                queueName: "myerror",
                account: Environment.UserName);
            Assert.IsTrue(MessageQueue.Exists(@".\private$\myerror"));

            QueueCreationUtils.CreateQueue(
                queueName: "myaudit",
                account: Environment.UserName);
            Assert.IsTrue(MessageQueue.Exists(@".\private$\myaudit"));
        }
    }
}
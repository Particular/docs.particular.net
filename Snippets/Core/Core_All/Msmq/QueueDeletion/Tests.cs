namespace CoreAll.Msmq.QueueDeletion
{
    using System;
    using System.Messaging;
    using CoreAll.Msmq.QueueCreation;
    using NUnit.Framework;

    [TestFixture]
    [Explicit]
    public class Tests
    {

        [Test]
        public void DeleteAllQueues()
        {
            QueueDeletionUtils.DeleteAllQueues();
        }

        [Test]
        public void DeleteQueuesForEndpoint()
        {
            CreateEndpointQueues.CreateQueuesForEndpoint(
                endpointName: "myendpoint",
                account: Environment.UserName);
            DeleteEndpointQueues.DeleteQueuesForEndpoint("myendpoint");
            Assert.IsFalse(MessageQueue.Exists(@".\private$\myendpoint"));

            QueueCreationUtils.CreateQueue(
                queueName: "myerror",
                account: Environment.UserName);
            QueueDeletionUtils.DeleteQueue(queueName: "myerror");
            Assert.IsFalse(MessageQueue.Exists(@".\private$\myerror"));

            QueueCreationUtils.CreateQueue(
                queueName: "myaudit",
                account: Environment.UserName);
            QueueDeletionUtils.DeleteQueue(queueName: "myaudit");
            Assert.IsFalse(MessageQueue.Exists(@".\private$\myaudit"));
        }
    }

}
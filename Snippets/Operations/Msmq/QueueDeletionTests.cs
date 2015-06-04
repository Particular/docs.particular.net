namespace Operations.Msmq
{
    using System;
    using System.Messaging;
    using NUnit.Framework;

    [TestFixture]
    [Explicit]
    public class QueueDeletionTests
    {

        [Test]
        public void DeleteAllQueues()
        {
            QueueDeletion.DeleteAllQueues();
        }

        [Test]
        public void DeleteQueuesForEndpoint()
        {
            QueueCreation.CreateQueuesForEndpoint(
                endpointName: "myendpoint",
                account: Environment.UserName);
            QueueDeletion.DeleteQueuesForEndpoint("myendpoint");
            Assert.IsFalse(MessageQueue.Exists(@".\private$\myendpoint"));

            QueueCreation.CreateQueue(
                queueName: "myerror",
                account: Environment.UserName);
            QueueDeletion.DeleteQueue(queueName: "myerror");
            Assert.IsFalse(MessageQueue.Exists(@".\private$\myerror"));

            QueueCreation.CreateQueue(
                queueName: "myaudit",
                account: Environment.UserName);
            QueueDeletion.DeleteQueue(queueName: "myaudit");
            Assert.IsFalse(MessageQueue.Exists(@".\private$\myaudit"));
        }
    }

}
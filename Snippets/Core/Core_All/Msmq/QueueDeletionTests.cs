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
            QueueDeletionUtils.DeleteAllQueues();
        }

        [Test]
        public void DeleteQueuesForEndpoint()
        {
            //TODO: move this to separate version specific queue deletion tests
            /*
            QueueCreation.CreateQueue(
                queueName: "myendpoint",
                account: Environment.UserName);
            QueueDeletion.DeleteQueue("myendpoint");
            Assert.IsFalse(MessageQueue.Exists(@".\private$\myendpoint"));
            */

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
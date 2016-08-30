namespace Operations.Msmq
{
    using System;
    using System.Messaging;
    using NUnit.Framework;

    [TestFixture]
    [Explicit]
    public class QueueCreationTests
    {
        [SetUp]
        [TearDown]
        public void Setup()
        {
            QueueDeletionUtils.DeleteQueue("myerror");
            QueueDeletionUtils.DeleteQueue("myaudit");
        }

        [Test]
        public void CreateQueues()
        {
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
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
            QueueDeletion.DeleteQueuesForEndpoint("myendpoint");
            QueueDeletion.DeleteQueue("myerror");
            QueueDeletion.DeleteQueue("myaudit");
        }

        [Test]
        public void CreateQueuesForEndpoint()
        {
            QueueCreation.CreateQueuesForEndpoint(
                endpointName: "myendpoint",
                account: Environment.UserName);

            Assert.IsTrue(MessageQueue.Exists(@".\private$\myendpoint"));

            QueueCreation.CreateQueue(
                queueName: "myerror",
                account: Environment.UserName);
            Assert.IsTrue(MessageQueue.Exists(@".\private$\myerror"));

            QueueCreation.CreateQueue(
                queueName: "myaudit",
                account: Environment.UserName);
            Assert.IsTrue(MessageQueue.Exists(@".\private$\myaudit"));
        }
    }
}
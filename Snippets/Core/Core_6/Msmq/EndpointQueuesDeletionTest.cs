namespace Core6.Msmq
{
    using System;
    using System.Messaging;
    using NUnit.Framework;
    using Operations.Msmq;

    [TestFixture]
    [Explicit]
    public class EndpointQueuesDeletionTest
    {
        [Test]
        public void DeleteAllQueues()
        {
            QueueDeletionUtils.DeleteAllQueues();
        }

        [Test]
        public void DeleteQueuesForEndpoint()
        {
            EndpointQueuesCreation.CreateQueuesForEndpoint(
                endpointName: "myendpoint",
                instanceId: "myinstance",
                account: Environment.UserName);

            EndpointQueuesDeletion.DeleteQueuesForEndpoint(
                endpointName: "myendpoint",
                instanceId: "myinstance");

            Assert.IsFalse(MessageQueue.Exists(@".\private$\myendpoint"));
        }
    }
}
using System;
using NUnit.Framework;

namespace Operations.Msmq.Tests
{
    [TestFixture]
    [Explicit]
    public class QueueCreationTests
    {
        [Test]
        public void CreateQueuesForEndpoint()
        {
            QueueCreation.CreateQueuesForEndpoint(
                endpointName: "myendpoint",
                account: Environment.UserName);

            QueueCreation.CreateQueue(
                queueName: "error",
                account: Environment.UserName);

            QueueCreation.CreateQueue(
                queueName: "audit",
                account: Environment.UserName);
        }

        [Test]
        public void DeleteAllQueues()
        {
            QueueCreation.DeleteAllQueues();
        }

        [Test]
        public void DeleteQueuesForEndpoint()
        {
            QueueCreation.DeleteQueuesForEndpoint("myendpoint");
        }
    }

}
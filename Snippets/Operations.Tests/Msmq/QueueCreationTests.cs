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
            #region msmq-create-queues-endpoint-usage
            QueueCreation.CreateQueuesForEndpoint(
                endpointName: "myendpoint",
                account: Environment.UserName);

            #endregion

            #region msmq-create-queues-shared-usage
            QueueCreation.CreateQueue(
                queueName: "error",
                account: Environment.UserName);

            QueueCreation.CreateQueue(
                queueName: "audit",
                account: Environment.UserName);
            #endregion
        }

        [Test]
        public void DeleteAllQueues()
        {
            #region msmq-delete-all-queues
            QueueCreation.DeleteAllQueues();
            #endregion
        }

        [Test]
        public void DeleteQueuesForEndpoint()
        {
            #region msmq-delete-queues-endpoint-usage
            QueueCreation.DeleteQueuesForEndpoint("myendpoint");
            #endregion

            #region msmq-delete-queues-shared-usage
            QueueCreation.DeleteQueue(queueName: "error");
            QueueCreation.DeleteQueue(queueName: "audit");
            #endregion
        }
    }

}
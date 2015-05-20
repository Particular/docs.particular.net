using System;
using NUnit.Framework;

namespace Operations.Msmq.Tests
{
    [TestFixture]
    [Explicit]
    public class QueueCreationTests
    {
        [Test]
        public void Foo()
        {
            QueueCreation.CreateAllForEndpoint(
                endpointName: "myendpoint",
                account: Environment.UserName);

            QueueCreation.CreateLocalQueue(
                queueName: "error",
                account: Environment.UserName);

            QueueCreation.CreateLocalQueue(
                queueName: "audit",
                account: Environment.UserName);

        }
    }

}
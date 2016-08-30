namespace Core6.Msmq
{
    using System;
    using System.Messaging;
    using NUnit.Framework;

    [TestFixture]
    [Explicit]
    public class EndpointQueuesCreationTests
    {
        [SetUp]
        [TearDown]
        public void Setup()
        {
            EndpointQueuesDeletion.DeleteQueuesForEndpoint("myendpoint", "myinstance");
        }

        [Test]
        public void CreateQueues()
        {
            EndpointQueuesCreation.CreateQueuesForEndpoint(
                endpointName: "myendpoint",
                instanceId: "myinstance",
                account: Environment.UserName);

            Assert.IsTrue(MessageQueue.Exists(@".\private$\myendpoint"));
        }
    }
}
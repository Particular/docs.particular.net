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
            EndpointQueuesDeletion.DeleteQueuesForEndpoint("myendpoint");
        }

        [Test]
        public void CreateQueues()
        {
            EndpointQueuesCreation.CreateQueuesForEndpoint(
                endpointName: "myendpoint",
                account: Environment.UserName);

            Assert.IsTrue(MessageQueue.Exists(@".\private$\myendpoint"));
        }
    }
}
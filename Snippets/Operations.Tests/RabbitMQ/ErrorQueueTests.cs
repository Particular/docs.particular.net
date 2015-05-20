using NUnit.Framework;

namespace Operations.RabbitMQ.Tests
{
    using System;

    [TestFixture]
    [Explicit]
    public class ErrorQueueTests
    {
        [Test]
        public void Foo()
        {

            ErrorQueue.ReturnMessageToSourceQueue(
                errorQueueMachine: Environment.MachineName, 
                errorQueueName: "error",
                userName:"admin",
                password:"password",
                messageId: @"6698f196-bd50-4f3c-8819-a49e0163d57b");
        }
    }

}
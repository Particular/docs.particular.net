using System;
using NUnit.Framework;

[TestFixture]
[Explicit]
public class ErrorQueueTests
{
    [Test]
    public void ReturnMessageToSourceQueue()
    {
        ErrorQueue.ReturnMessageToSourceQueue(
            errorQueueMachine: Environment.MachineName,
            errorQueueName: "error",
            userName:"admin",
            password:"password",
            messageId: "6698f196-bd50-4f3c-8819-a49e0163d57b");
    }
}
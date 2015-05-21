using System;
using NUnit.Framework;

namespace Operations.Msmq.Tests
{
    [TestFixture]
    [Explicit]
    public class ErrorQueueTests
    {
        [Test]
        public void ReturnMessageToSourceQueue()
        {
            #region msmq-return-to-source-queue-usage
            ErrorQueue.ReturnMessageToSourceQueue(
                machineName: Environment.MachineName, 
                queueName: "error",
                msmqMessageId: @"c390a6fb-4fb5-46da-927d-a156f75739eb\15386");
            #endregion
        }
    }

}
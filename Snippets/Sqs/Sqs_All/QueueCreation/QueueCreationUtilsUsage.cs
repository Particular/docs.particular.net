namespace SqsAll.QueueCreation
{
    using System;
    using System.Threading.Tasks;

    class QueueCreationUtilsUsage
    {

        async Task Usage()
        {

            #region sqs-create-queues-shared-usage

            await QueueCreationUtils.CreateQueue(
                    queueName: "error",
                    maxTimeToLive: TimeSpan.FromDays(2),
                    queueNamePrefix: "PROD");

            await QueueCreationUtils.CreateQueue(
                    queueName: "audit",
                    maxTimeToLive: TimeSpan.FromDays(2),
                    queueNamePrefix: "PROD");

            #endregion

            #region sqs-create-queues-shared-usage-cloudformation

            await QueueCreationUtilsCloudFormation.CreateQueue(
                    queueName: "error",
                    templatePath: @".\QueueCreation.json",
                    maxTimeToLive: TimeSpan.FromDays(2),
                    queueNamePrefix: "PROD");

            await QueueCreationUtilsCloudFormation.CreateQueue(
                    queueName: "audit",
                    templatePath: @".\QueueCreation.json",
                    maxTimeToLive: TimeSpan.FromDays(2),
                    queueNamePrefix: "PROD");

            #endregion
        }

    }

}
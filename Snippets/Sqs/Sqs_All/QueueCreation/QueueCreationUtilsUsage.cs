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
                maxTimeToLive: TimeSpan.FromDays(2))
                .ConfigureAwait(false);

            await QueueCreationUtils.CreateQueue(
                queueName: "audit",
                maxTimeToLive: TimeSpan.FromDays(2))
                .ConfigureAwait(false);

            #endregion
        }

    }

}
namespace SqsAll.QueueDeletion
{
    using System.Threading.Tasks;

    class QueueDeletionUtilsUsage
    {

        async Task Usage()
        {
            #region sqs-delete-queues-shared-usage

            await QueueDeletionUtils.DeleteQueue(queueName: "error", queueNamePrefix: "PROD")
                .ConfigureAwait(false);
            await QueueDeletionUtils.DeleteQueue(queueName: "audit", queueNamePrefix: "PROD")
                .ConfigureAwait(false);

            #endregion

            #region sqs-delete-queues-shared-usage-cloudformation

            await QueueDeletionUtilsCloudFormation.DeleteQueue(queueName: "error", queueNamePrefix: "PROD")
                .ConfigureAwait(false);
            await QueueDeletionUtilsCloudFormation.DeleteQueue(queueName: "audit", queueNamePrefix: "PROD")
                .ConfigureAwait(false);

            #endregion
        }

    }

}
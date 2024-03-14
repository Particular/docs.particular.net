namespace SqsAll.QueueDeletion
{
    using System.Threading.Tasks;

    class QueueDeletionUtilsUsage
    {

        async Task Usage()
        {
            #region sqs-delete-queues-shared-usage

            await QueueDeletionUtils.DeleteQueue(queueName: "error", queueNamePrefix: "PROD");
            await QueueDeletionUtils.DeleteQueue(queueName: "audit", queueNamePrefix: "PROD");

            #endregion

            #region sqs-delete-queues-shared-usage-cloudformation

            await QueueDeletionUtilsCloudFormation.DeleteQueue(queueName: "error", queueNamePrefix: "PROD");
            await QueueDeletionUtilsCloudFormation.DeleteQueue(queueName: "audit", queueNamePrefix: "PROD");

            #endregion
        }

    }

}
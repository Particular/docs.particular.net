namespace SqsAll.QueueDeletion
{
    using System.Threading.Tasks;

    class QueueDeletionUtilsUsage
    {

        async Task Usage()
        {
            #region sqs-delete-queues-shared-usage

            await QueueDeletionUtils.DeleteQueue(queueName: "error")
                .ConfigureAwait(false);
            await QueueDeletionUtils.DeleteQueue(queueName: "audit")
                .ConfigureAwait(false);

            #endregion
        }

    }

}
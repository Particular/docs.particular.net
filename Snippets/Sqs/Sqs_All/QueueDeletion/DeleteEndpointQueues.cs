namespace SqsAll.QueueDeletion
{
    using System.Threading.Tasks;

    public class DeleteEndpointQueues
    {

        async Task Usage()
        {
            #region sqs-delete-queues-endpoint-usage [6,)

            await DeleteQueuesForEndpoint(
                endpointName: "myendpoint", 
                queueNamePrefix: "PROD", 
                preTruncateQueueNames: true)
            .ConfigureAwait(false);

            #endregion

            #region sqs-delete-queues-endpoint-usage [,5)

            await DeleteQueuesForEndpoint(
                endpointName: "myendpoint",
                queueNamePrefix: "PROD",
                preTruncateQueueNames: true, 
                includeRetries: true)
            .ConfigureAwait(false);

            #endregion
        }
        #region sqs-delete-queues-for-endpoint

        public static async Task DeleteQueuesForEndpoint(string endpointName, string queueNamePrefix = null, bool preTruncateQueueNames = false, bool includeRetries = false)
        {
            // main queue
            await QueueDeletionUtils.DeleteQueue(endpointName, queueNamePrefix, preTruncateQueueNames)
                .ConfigureAwait(false);

            // timeout queue
            await QueueDeletionUtils.DeleteQueue($"{endpointName}.Timeouts", queueNamePrefix, preTruncateQueueNames)
                .ConfigureAwait(false);

            // timeout dispatcher queue
            await QueueDeletionUtils.DeleteQueue($"{endpointName}.TimeoutsDispatcher", queueNamePrefix, preTruncateQueueNames)
                .ConfigureAwait(false);

            // retries queue
            if (includeRetries)
            {
                await QueueDeletionUtils.DeleteQueue($"{endpointName}.Retries", queueNamePrefix, preTruncateQueueNames)
                    .ConfigureAwait(false);
            }
        }

        #endregion

    }

}
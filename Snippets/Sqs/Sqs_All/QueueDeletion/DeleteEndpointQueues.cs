namespace SqsAll.QueueDeletion
{
    using System.Threading.Tasks;

    public class DeleteEndpointQueues
    {

        async Task Usage()
        {
            #region sqs-delete-queues-endpoint-usage [6,)

            await DeleteQueuesForEndpoint("myendpoint")
                .ConfigureAwait(false);

            #endregion

            #region sqs-delete-queues-endpoint-usage [,5)

            await DeleteQueuesForEndpoint("myendpoint", includeRetries: true)
                .ConfigureAwait(false);

            #endregion
        }
        #region sqs-delete-queues-for-endpoint

        public static async Task DeleteQueuesForEndpoint(string endpointName, bool includeRetries = false)
        {
            // main queue
            await QueueDeletionUtils.DeleteQueue(endpointName)
                .ConfigureAwait(false);

            // timeout queue
            await QueueDeletionUtils.DeleteQueue($"{endpointName}.timeouts")
                .ConfigureAwait(false);

            // timeout dispatcher queue
            await QueueDeletionUtils.DeleteQueue($"{endpointName}.timeoutsdispatcher")
                .ConfigureAwait(false);

            // retries queue
            if (includeRetries)
            {
                await QueueDeletionUtils.DeleteQueue($"{endpointName}.retries")
                    .ConfigureAwait(false);
            }
        }

        #endregion

    }

}
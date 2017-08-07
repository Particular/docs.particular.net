namespace SqsAll.QueueDeletion
{
    using System.Threading.Tasks;

    public class DeleteEndpointQueues
    {

        async Task Usage()
        {
            #region sqs-delete-queues-endpoint-usage

            await DeleteQueuesForEndpoint("myendpoint")
                .ConfigureAwait(false);

            #endregion
        }
        #region sqs-delete-queues-for-endpoint

        public static async Task DeleteQueuesForEndpoint(string endpointName)
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
            // TODO: Only required in Versions 5 and below
            await QueueDeletionUtils.DeleteQueue($"{endpointName}.retries")
                .ConfigureAwait(false);
        }

        #endregion

    }

}
namespace SqsAll.QueueDeletion
{
    using System.Threading.Tasks;

    public class DeleteEndpointQueues
    {

        async Task Usage()
        {
            #region sqs-delete-queues-endpoint-usage

            await DeleteQueuesForEndpoint(
                endpointName: "myendpoint", 
                queueNamePrefix: "PROD",
                includeRetries: true /* required for V5 and below */)
            .ConfigureAwait(false);

            #endregion
        }

        #region sqs-delete-queues-for-endpoint

        public static async Task DeleteQueuesForEndpoint(string endpointName, string queueNamePrefix = null, bool includeRetries = false, string delayedDeliveryMethod = "Native")
        {
            switch (delayedDeliveryMethod)
            {
                case "TimeoutManager":

                    // timeout queue
                    await QueueDeletionUtils.DeleteQueue($"{endpointName}.Timeouts", queueNamePrefix)
                        .ConfigureAwait(false);

                    // timeout dispatcher queue
                    await QueueDeletionUtils.DeleteQueue($"{endpointName}.TimeoutsDispatcher", queueNamePrefix)
                        .ConfigureAwait(false);

                    break;
                case "UnrestrictedDelayedDelivery":

                    await QueueDeletionUtils.DeleteQueue($"{endpointName}-delay.fifo", queueNamePrefix)
                        .ConfigureAwait(false);

                    break;
            }

            // main queue
            await QueueDeletionUtils.DeleteQueue(endpointName, queueNamePrefix)
                .ConfigureAwait(false);

            // retries queue
            if (includeRetries)
            {
                await QueueDeletionUtils.DeleteQueue($"{endpointName}.Retries", queueNamePrefix)
                    .ConfigureAwait(false);
            }
        }

        #endregion

    }
}
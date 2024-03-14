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
                includeRetries: true /* required for V5 and below */);

            #endregion
        }

        #region sqs-delete-queues-for-endpoint

        public static async Task DeleteQueuesForEndpoint(string endpointName, string queueNamePrefix = null, bool includeRetries = false, string delayedDeliveryMethod = "Native")
        {
            switch (delayedDeliveryMethod)
            {
                case "TimeoutManager":

                    // timeout queue
                    await QueueDeletionUtils.DeleteQueue($"{endpointName}.Timeouts", queueNamePrefix);

                    // timeout dispatcher queue
                    await QueueDeletionUtils.DeleteQueue($"{endpointName}.TimeoutsDispatcher", queueNamePrefix);

                    break;
                case "UnrestrictedDelayedDelivery":

                    await QueueDeletionUtils.DeleteQueue($"{endpointName}-delay.fifo", queueNamePrefix);

                    break;
            }

            // main queue
            await QueueDeletionUtils.DeleteQueue(endpointName, queueNamePrefix);

            // retries queue
            if (includeRetries)
            {
                await QueueDeletionUtils.DeleteQueue($"{endpointName}.Retries", queueNamePrefix);
            }
        }

        #endregion

    }
}
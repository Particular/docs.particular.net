namespace SqsAll.QueueCreation
{
    using System;
    using System.Threading.Tasks;

    static class CreateEndpointQueues
    {

        static async Task Usage()
        {
            #region sqs-create-queues-endpoint-usage [6,)

            await CreateQueuesForEndpoint(
                    endpointName: "myendpoint",
                    maxTimeToLive: TimeSpan.FromDays(2))
                .ConfigureAwait(false);

            #endregion

            #region sqs-create-queues-endpoint-usage [,5)

            await CreateQueuesForEndpoint(
                    endpointName: "myendpoint",
                    maxTimeToLive: TimeSpan.FromDays(2),
                    includeRetries: true)
                .ConfigureAwait(false);

            #endregion
        }

        #region sqs-create-queues-for-endpoint

        public static async Task CreateQueuesForEndpoint(string endpointName, TimeSpan? maxTimeToLive = null, bool includeRetries = false)
        {
            // main queue
            await QueueCreationUtils.CreateQueue(endpointName, maxTimeToLive)
                .ConfigureAwait(false);

            // timeout queue
            await QueueCreationUtils.CreateQueue($"{endpointName}.Timeouts", maxTimeToLive)
                .ConfigureAwait(false);

            // timeout dispatcher queue
            await QueueCreationUtils.CreateQueue($"{endpointName}.TimeoutsDispatcher", maxTimeToLive)
                .ConfigureAwait(false);

            // retries queue
            if (includeRetries)
            {
                await QueueCreationUtils.CreateQueue($"{endpointName}.Retries", maxTimeToLive)
                    .ConfigureAwait(false);
            }
        }

        #endregion
    }

}
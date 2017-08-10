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
                    maxTimeToLive: TimeSpan.FromDays(2),
                    queueNamePrefix: "PROD")
                .ConfigureAwait(false);

            #endregion

            #region sqs-create-queues-endpoint-usage [,5)

            await CreateQueuesForEndpoint(
                    endpointName: "myendpoint",
                    maxTimeToLive: TimeSpan.FromDays(2),
                    queueNamePrefix: "PROD",
                    includeRetries: true)
                .ConfigureAwait(false);

            #endregion
        }

        #region sqs-create-queues-for-endpoint

        public static async Task CreateQueuesForEndpoint(string endpointName, TimeSpan? maxTimeToLive = null, string queueNamePrefix = null, bool includeRetries = false)
        {
            // main queue
            await QueueCreationUtils.CreateQueue(endpointName, maxTimeToLive, queueNamePrefix)
                .ConfigureAwait(false);

            // timeout queue
            await QueueCreationUtils.CreateQueue($"{endpointName}.Timeouts", maxTimeToLive, queueNamePrefix)
                .ConfigureAwait(false);

            // timeout dispatcher queue
            await QueueCreationUtils.CreateQueue($"{endpointName}.TimeoutsDispatcher", maxTimeToLive, queueNamePrefix)
                .ConfigureAwait(false);

            // retries queue
            if (includeRetries)
            {
                await QueueCreationUtils.CreateQueue($"{endpointName}.Retries", maxTimeToLive, queueNamePrefix)
                    .ConfigureAwait(false);
            }
        }

        #endregion
    }

}
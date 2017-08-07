namespace SqsAll.QueueCreation
{
    using System;
    using System.Threading.Tasks;

    class CreateEndpointQueues
    {

        async Task Usage()
        {
            #region sqs-create-queues-endpoint-usage

            await CreateQueuesForEndpoint(
                    endpointName: "myendpoint",
                    maxTimeToLive: TimeSpan.FromDays(2))
                .ConfigureAwait(false);

            #endregion
        }

        #region sqs-create-queues-for-endpoint

        public static async Task CreateQueuesForEndpoint(string endpointName, TimeSpan? maxTimeToLive = null)
        {
            // main queue
            await QueueCreationUtils.CreateQueue(endpointName, maxTimeToLive)
                .ConfigureAwait(false);

            // timeout queue
            await QueueCreationUtils.CreateQueue($"{endpointName}.timeouts", maxTimeToLive)
                .ConfigureAwait(false);

            // timeout dispatcher queue
            await QueueCreationUtils.CreateQueue($"{endpointName}.timeoutsdispatcher", maxTimeToLive)
                .ConfigureAwait(false);

            // retries queue
            // TODO: Only required in Versions 5 and below
            await QueueCreationUtils.CreateQueue($"{endpointName}.retries", maxTimeToLive)
                .ConfigureAwait(false);
        }

        #endregion
    }

}
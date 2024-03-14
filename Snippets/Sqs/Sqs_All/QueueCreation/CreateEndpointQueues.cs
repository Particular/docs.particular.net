namespace SqsAll.QueueCreation
{
    using System;
    using System.Threading.Tasks;

    static class CreateEndpointQueues
    {

        static async Task Usage()
        {
            #region sqs-create-queues-endpoint-usage

            await CreateQueuesForEndpoint(
                    endpointName: "myendpoint",
                    maxTimeToLive: TimeSpan.FromDays(2),
                    queueNamePrefix: "PROD",
                    includeRetries: true /* required for V5 and below */);

            #endregion
        }

        #region sqs-create-queues-for-endpoint

        public static async Task CreateQueuesForEndpoint(string endpointName, TimeSpan? maxTimeToLive = null, string queueNamePrefix = null, bool includeRetries = false, string delayedDeliveryMethod = "Native")
        {
            switch (delayedDeliveryMethod)
            {
                case "TimeoutManager":

                    // timeout dispatcher queue
                    // This queue is created first because it has the longest name.
                    // If the endpointName and queueNamePrefix are too long this call will throw and no queues will be created.
                    // In this event, a shorter value for endpointName or queueNamePrefix should be used.
                    await QueueCreationUtils.CreateQueue($"{endpointName}.TimeoutsDispatcher", maxTimeToLive, queueNamePrefix);

                    // timeout queue
                    await QueueCreationUtils.CreateQueue($"{endpointName}.Timeouts", maxTimeToLive, queueNamePrefix);

                    break;
                case "UnrestrictedDelayedDelivery":

                    await QueueCreationUtils.CreateQueue($"{endpointName}-delay.fifo", maxTimeToLive, queueNamePrefix);

                    break;
            }

            // main queue
            await QueueCreationUtils.CreateQueue(endpointName, maxTimeToLive, queueNamePrefix);


            // retries queue
            if (includeRetries)
            {
                await QueueCreationUtils.CreateQueue($"{endpointName}.Retries", maxTimeToLive, queueNamePrefix);
            }
        }

        #endregion
    }
}
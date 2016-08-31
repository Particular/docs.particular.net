using System;

namespace Rabbit_All.QueueCreation
{
    class CreateEndpointQueues_4_and_above
    {

        #region rabbit-create-queues-for-endpoint [4,]

        public static void CreateQueuesForEndpoint(string uri, string endpointName, bool durableMessages, bool createExchanges)
        {
            // main queue
            QueueCreationUtils.CreateQueue(uri, endpointName, durableMessages, createExchanges);

            // callback queue
            QueueCreationUtils.CreateQueue(uri, $"{endpointName}.{Environment.MachineName}", durableMessages, createExchanges);

            // timeout queue
            QueueCreationUtils.CreateQueue(uri, $"{endpointName}.Timeouts", durableMessages, createExchanges);

            // timeout dispatcher queue
            QueueCreationUtils.CreateQueue(uri, $"{endpointName}.TimeoutsDispatcher", durableMessages, createExchanges);
        }


        #endregion
    }
}
using System;

namespace Rabbit_All.QueueCreation
{
    class CreateEndpointQueues
    {

        CreateEndpointQueues()
        {
            #region rabbit-create-queues-endpoint-usage

            CreateQueuesForEndpoint(
                uri: "amqp://guest:guest@localhost:5672",
                endpointName: "myendpoint",
                durableMessages: true,
                createExchanges: true);

            #endregion
        }

        #region rabbit-create-queues-for-endpoint

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

            // retries queue
            // TODO: Only required in Versions 3 and below
            QueueCreationUtils.CreateQueue(uri, $"{endpointName}.Retries", durableMessages, createExchanges);

        }


        #endregion
    }
}
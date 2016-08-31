namespace CoreAll.Msmq.QueueCreation
{
    using System;

    class CreateEndpointQueues
    {

        CreateEndpointQueues()
        {
            #region msmq-create-queues-endpoint-usage

            CreateQueuesForEndpoint(
                endpointName: "myendpoint",
                account: Environment.UserName);

            #endregion
        }

        #region msmq-create-queues-for-endpoint

        public static void CreateQueuesForEndpoint(string endpointName, string account)
        {
            // main queue
            QueueCreationUtils.CreateQueue(endpointName, account);

            // retries queue
            QueueCreationUtils.CreateQueue($"{endpointName}.retries", account);

            // timeout queue
            QueueCreationUtils.CreateQueue($"{endpointName}.timeouts", account);

            // timeout dispatcher queue
            QueueCreationUtils.CreateQueue($"{endpointName}.timeoutsdispatcher", account);
        }

        #endregion
    }

}
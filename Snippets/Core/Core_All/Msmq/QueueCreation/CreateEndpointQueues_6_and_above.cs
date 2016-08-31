namespace CoreAll.Msmq.QueueCreation
{

    class CreateEndpointQueues_6_and_above
    {

        #region msmq-create-queues-for-endpoint [6,]

        public static void CreateQueuesForEndpoint(string endpointName, string account)
        {
            // main queue
            QueueCreationUtils.CreateQueue(endpointName, account);

            // timeout queue
            QueueCreationUtils.CreateQueue($"{endpointName}.timeouts", account);

            // timeout dispatcher queue
            QueueCreationUtils.CreateQueue($"{endpointName}.timeoutsdispatcher", account);
        }

        #endregion
    }

}
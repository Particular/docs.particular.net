namespace Core6.Msmq
{
    using System;
    using Operations.Msmq;

    public class EndpointQueuesCreation
    {
        #region msmq-create-queues-for-endpoint-usage

        public static void Usage()
        {
            CreateQueuesForEndpoint(
                endpointName: "myendpoint",
                instanceId: "myinstance",
                account: Environment.UserName);
        }

        #endregion

        #region msmq-create-queues-for-endpoint

        public static void CreateQueuesForEndpoint(string endpointName, string instanceId, string account)
        {
            // main queue
            QueueCreationUtils.CreateQueue(endpointName, account);

            // instance specific queue
            if (string.IsNullOrEmpty(instanceId) == false)
            {
                // instance specific queue
                QueueCreationUtils.CreateQueue($"{endpointName}.{instanceId}", account);
            }

            // timeout queue
            QueueCreationUtils.CreateQueue($"{endpointName}.timeouts", account);

            // timeout dispatcher queue
            QueueCreationUtils.CreateQueue($"{endpointName}.timeoutsdispatcher", account);
        }

        #endregion

    }
}
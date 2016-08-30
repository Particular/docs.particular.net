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
                account: Environment.UserName);
        }

        #endregion

        #region msmq-create-queues-for-endpoint

        public static void CreateQueuesForEndpoint(string endpointName, string account)
        {
            // main queue
            QueueCreationUtils.CreateQueue(endpointName, account);

            // retries queue
            QueueCreationUtils.CreateQueue($"{endpointName}.retires", account);

            // timeout queue
            QueueCreationUtils.CreateQueue($"{endpointName}.timeouts", account);

            // timeout dispatcher queue
            QueueCreationUtils.CreateQueue($"{endpointName}.timeoutsdispatcher", account);
        }

        #endregion

    }
}
namespace Core6.Msmq
{
    using Operations.Msmq;

    public static class EndpointQueuesDeletion
    {
        public static void Usage()
        {
            #region msmq-delete-queues-endpoint-usage

            DeleteQueuesForEndpoint("myendpoint");

            #endregion
        }

        #region msmq-delete-queues-for-endpoint

        public static void DeleteQueuesForEndpoint(string endpointName)
        {
            // main queue
            QueueDeletionUtils.DeleteQueue(endpointName);

            //retries queue
            QueueDeletionUtils.DeleteQueue($"{endpointName}.retries");

            // timeout queue
            QueueDeletionUtils.DeleteQueue($"{endpointName}.timeouts");

            // timeout dispatcher queue
            QueueDeletionUtils.DeleteQueue($"{endpointName}.timeoutsdispatcher");
        }

        #endregion
    }
}
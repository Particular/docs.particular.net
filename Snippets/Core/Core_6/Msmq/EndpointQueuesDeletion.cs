namespace Core6.Msmq
{
    using Operations.Msmq;

    public static class EndpointQueuesDeletion
    {
        public static void Usage()
        {
            #region msmq-delete-queues-endpoint-usage

            DeleteQueuesForEndpoint("myendpoint", "myinstance");

            #endregion
        }

        #region msmq-delete-queues-for-endpoint

        public static void DeleteQueuesForEndpoint(string endpointName, string instanceId)
        {
            // main queue
            QueueDeletionUtils.DeleteQueue(endpointName);

            //instance specific queue
            if (string.IsNullOrEmpty(instanceId) == false)
            {
                QueueDeletionUtils.DeleteQueue($"{endpointName}.{instanceId}");
            }

            // timeout queue
            QueueDeletionUtils.DeleteQueue($"{endpointName}.timeouts");

            // timeout dispatcher queue
            QueueDeletionUtils.DeleteQueue($"{endpointName}.timeoutsdispatcher");
        }

        #endregion
    }
}
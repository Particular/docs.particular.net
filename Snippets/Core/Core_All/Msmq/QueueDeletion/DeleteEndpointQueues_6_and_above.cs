namespace CoreAll.Msmq.QueueDeletion
{

    public class DeleteEndpointQueues_6_and_above
    {

        #region msmq-delete-queues-for-endpoint [6,]

        public static void DeleteQueuesForEndpoint(string endpointName)
        {
            // main queue
            QueueDeletionUtils.DeleteQueue(endpointName);

            // timeout queue
            QueueDeletionUtils.DeleteQueue($"{endpointName}.timeouts");

            // timeout dispatcher queue
            QueueDeletionUtils.DeleteQueue($"{endpointName}.timeoutsdispatcher");
        }

        #endregion

    }

}
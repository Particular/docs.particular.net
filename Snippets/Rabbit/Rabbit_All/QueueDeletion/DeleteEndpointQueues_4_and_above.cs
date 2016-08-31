namespace Rabbit_All.QueueDeletion
{
    using System;
    public class DeleteEndpointQueues_4_and_above
    {

        #region rabbit-delete-queues-for-endpoint [4,]
        public static void DeleteQueuesForEndpoint(string uri, string endpointName)
        {
            // main queue
            QueueDeletionUtils.DeleteQueue(uri, endpointName);

            // callback queue
            QueueDeletionUtils.DeleteQueue(uri, $"{endpointName}.{Environment.MachineName}");

            // timeout queue
            QueueDeletionUtils.DeleteQueue(uri, $"{endpointName}.Timeouts");

            // timeout dispatcher queue
            QueueDeletionUtils.DeleteQueue(uri, $"{endpointName}.TimeoutsDispatcher");
        }
        #endregion
    }
}
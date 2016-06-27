namespace Operations.Msmq
{
    using System;
    using System.Messaging;

    public static class QueueDeletion
    {

        static void Usage()
        {
            #region msmq-delete-queues-endpoint-usage

            DeleteQueuesForEndpoint("myendpoint");

            #endregion

            #region msmq-delete-queues-shared-usage

            DeleteQueue(queueName: "error");
            DeleteQueue(queueName: "audit");

            #endregion
        }

        #region msmq-delete-queues

        public static void DeleteAllQueues()
        {
            MessageQueue[] machineQueues = MessageQueue.GetPrivateQueuesByMachine(".");
            foreach (var q in machineQueues)
            {
                MessageQueue.Delete(q.Path);
            }
        }

        public static void DeleteQueue(string queueName)
        {
            var path = $@"{Environment.MachineName}\private$\{queueName}";
            if (MessageQueue.Exists(path))
            {
                MessageQueue.Delete(path);
            }
        }

        public static void DeleteQueuesForEndpoint(string endpointName)
        {
            //main queue
            DeleteQueue(endpointName);

            //retries queue
            DeleteQueue($"{endpointName}.retries");

            //timeout queue
            DeleteQueue($"{endpointName}.timeouts");

            //timeout dispatcher queue
            DeleteQueue($"{endpointName}.timeoutsdispatcher");
        }

        #endregion

    }

}
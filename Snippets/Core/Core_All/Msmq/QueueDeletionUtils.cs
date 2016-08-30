namespace Operations.Msmq
{
    using System;
    using System.Messaging;

    public static class QueueDeletionUsage
    {

        static void Usage()
        {
            #region msmq-delete-queues-shared-usage

            QueueDeletionUtils.DeleteQueue(queueName: "error");
            QueueDeletionUtils.DeleteQueue(queueName: "audit");

            #endregion
        }
    }

    #region msmq-delete-queues-shared

    public static class QueueDeletionUtils
    {
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

    }

    #endregion

}
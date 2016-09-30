namespace CoreAll.Msmq.QueueDeletion
{
    using System;
    using System.Messaging;

    #region msmq-delete-queues

    public static class QueueDeletionUtils
    {
        public static void DeleteAllQueues()
        {
            var machineQueues = MessageQueue.GetPrivateQueuesByMachine(".");
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

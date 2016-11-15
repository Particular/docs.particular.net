namespace CoreAll.Msmq.QueueDeletion
{

    class QueueDeletionUtilsUsage
    {

        QueueDeletionUtilsUsage()
        {
            #region msmq-delete-queues-shared-usage

            QueueDeletionUtils.DeleteQueue(queueName: "error");
            QueueDeletionUtils.DeleteQueue(queueName: "audit");

            #endregion
        }

    }

}
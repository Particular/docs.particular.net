namespace Rabbit_All.QueueDeletion
{
    class QueueDeletionUtilsUsage
    {

        QueueDeletionUtilsUsage()
        {

            #region rabbit-delete-queues-shared-usage

            QueueDeletionUtils.DeleteQueue(
                uri: "amqp://guest:guest@localhost:5672",
                queueName: "error");
            QueueDeletionUtils.DeleteQueue(
                uri: "amqp://guest:guest@localhost:5672",
                queueName: "audit");

            #endregion
        }
    }
}
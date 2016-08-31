
namespace Rabbit_All.QueueCreation
{
    class QueueCreationUtilsUsage
    {

        QueueCreationUtilsUsage()
        {

            #region rabbit-create-queues-shared-usage

            QueueCreationUtils.CreateQueue(
                uri: "amqp://guest:guest@localhost:5672",
                queueName: "error",
                durableMessages: true,
                createExchange: true);

            QueueCreationUtils.CreateQueue(
                uri: "amqp://guest:guest@localhost:5672",
                queueName: "audit",
                durableMessages: true,
                createExchange: true);

            #endregion
        }

    }
}
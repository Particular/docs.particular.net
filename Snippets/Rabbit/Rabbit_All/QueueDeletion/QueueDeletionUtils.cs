using RabbitMQ.Client;

namespace Rabbit_All.QueueDeletion
{
    using System;

    #region rabbit-delete-queues

    public static class QueueDeletionUtils
    {
        public static void DeleteQueue(string uri, string queueName)
        {
            var connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(uri)
            };

            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueUnbind(queueName, queueName, string.Empty, null);
                channel.ExchangeDelete(queueName);
                channel.QueueDelete(queueName);
            }
        }
    }

    #endregion
}
namespace Rabbit_All.Version_6
{
    using System;
    using RabbitMQ.Client;

    public class QueueDeletion
    {
        #region rabbit-delete-queues

        public static void DeleteQueue(string uri, string queueName)
        {
            var connectionFactory = new ConnectionFactory
            {
                Uri = uri,
            };

            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueUnbind(queueName, queueName, string.Empty, null);
                channel.ExchangeDelete(queueName);
                channel.QueueDelete(queueName);
            }
        }

        public static void DeleteQueuesForEndpoint(string uri, string endpointName)
        {
            // main queue
            DeleteQueue(uri, endpointName);

            // callback queue
            DeleteQueue(uri, $"{endpointName}.{Environment.MachineName}");

            // timeout queue
            DeleteQueue(uri, $"{endpointName}.Timeouts");

            // timeout dispatcher queue
            DeleteQueue(uri, $"{endpointName}.TimeoutsDispatcher");
        }

        #endregion
    }
}
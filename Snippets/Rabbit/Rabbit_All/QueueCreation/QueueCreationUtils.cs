using RabbitMQ.Client;

namespace Rabbit_All.QueueCreation
{
    using System;

    #region rabbit-create-queues
    public static class QueueCreationUtils
    {
        public static void CreateQueue(string uri, string queueName, bool durableMessages, bool createExchange)
        {
            var connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(uri)
            };

            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: queueName,
                    durable: durableMessages,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                if (createExchange)
                {
                    CreateExchange(channel, queueName, durableMessages);
                }
            }
        }

        static void CreateExchange(IModel channel, string queueName, bool durableMessages)
        {
            channel.ExchangeDeclare(queueName, ExchangeType.Fanout, durableMessages);
            channel.QueueBind(queueName, queueName, string.Empty);
        }
    }
#endregion
}

namespace Rabbit_All.Version_6
{
    using System;
    using RabbitMQ.Client;

    public static class QueueCreation
    {
        #region rabbit-create-queues

        public static void CreateQueuesForEndpoint(string uri, string endpointName, bool durableMessages, bool createExchanges)
        {
            // main queue
            CreateQueue(uri, endpointName, durableMessages, createExchanges);

            // callback queue
            CreateQueue(uri, $"{endpointName}.{Environment.MachineName}", durableMessages, createExchanges);

            // timeout queue
            CreateQueue(uri, $"{endpointName}.Timeouts", durableMessages, createExchanges);

            // timeout dispatcher queue
            CreateQueue(uri, $"{endpointName}.TimeoutsDispatcher", durableMessages, createExchanges);
        }

        public static void CreateQueue(string uri, string queueName, bool durableMessages, bool createExchange)
        {
            var connectionFactory = new ConnectionFactory
            {
                Uri = uri,
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

        #endregion
    }
}
namespace Operations.RabbitMQ
{
    using System;
    using global::RabbitMQ.Client;

    public static class QueueCreation
    {
        public static void CreateAllForEndpoint(string uri, string endpointName, bool durableMessages, bool createExchanges)
        {
            //main queue
            CreateLocalQueue(uri, endpointName, durableMessages, createExchanges);

            //callback queue
            CreateLocalQueue(uri, endpointName + "." + Environment.MachineName, durableMessages, createExchanges);

            //retries queue
            CreateLocalQueue(uri, endpointName + ".Retries", durableMessages, createExchanges);

            //timeout queue
            CreateLocalQueue(uri, endpointName + ".Timeouts", durableMessages, createExchanges);

            //timeout dispatcher queue
            CreateLocalQueue(uri, endpointName + ".TimeoutsDispatcher", durableMessages, createExchanges);
        }

        public static void CreateLocalQueue(string uri, string queueName, bool durableMessages, bool createExchange)
        {
            ConnectionFactory factory = new ConnectionFactory
            {
                Uri = uri,
            };

            using (IConnection conn = factory.CreateConnection())
            {
                IModel channel = conn.CreateModel();

                channel.QueueDeclare(
                    queue: queueName,
                    durable: durableMessages,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
                if (createExchange)
                {
                    CreateExhange(channel, queueName, durableMessages);
                }
            }
        }

        static void CreateExhange(IModel channel, string queueName, bool durableMessages)
        {
            channel.ExchangeDeclare(queueName, ExchangeType.Fanout, durableMessages);
            channel.QueueBind(queueName, queueName, string.Empty);
        }
    }
}
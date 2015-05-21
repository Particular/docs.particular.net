namespace Operations.RabbitMQ
{
    using System;
    using global::RabbitMQ.Client;

    public static class QueueCreation
    {

        public static void DeleteQueue(string uri, string queueName)
        {
            ConnectionFactory factory = new ConnectionFactory
            {
                Uri = uri,
            };
            using (IConnection conn = factory.CreateConnection())
            using (IModel channel = conn.CreateModel())
            {
                channel.QueueUnbind(queueName, queueName, string.Empty, null);
                channel.ExchangeDelete(queueName);
                channel.QueueDelete(queueName);
            }
        }

        public static void DeleteQueuesForEndpoint(string uri, string endpointName)
        {
            //main queue
            DeleteQueue(uri, endpointName);

            //callback queue
            DeleteQueue(uri, endpointName + "." + Environment.MachineName);

            //retries queue
            DeleteQueue(uri, endpointName + ".Retries");

            //timeout queue
            DeleteQueue(uri, endpointName + ".Timeouts");

            //timeout dispatcher queue
            DeleteQueue(uri, endpointName + ".TimeoutsDispatcher");
        }

        public static void CreateQueuesForEndpoint(string uri, string endpointName, bool durableMessages, bool createExchanges)
        {
            //main queue
            CreateQueue(uri, endpointName, durableMessages, createExchanges);

            //callback queue
            CreateQueue(uri, endpointName + "." + Environment.MachineName, durableMessages, createExchanges);

            //retries queue
            CreateQueue(uri, endpointName + ".Retries", durableMessages, createExchanges);

            //timeout queue
            CreateQueue(uri, endpointName + ".Timeouts", durableMessages, createExchanges);

            //timeout dispatcher queue
            CreateQueue(uri, endpointName + ".TimeoutsDispatcher", durableMessages, createExchanges);
        }

        public static void CreateQueue(string uri, string queueName, bool durableMessages, bool createExchange)
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
}
using System;
using global::RabbitMQ.Client;

public static class QueueCreation
{

    public static void Usage()
    {
        #region rabbit-create-queues-endpoint-usage

        CreateQueuesForEndpoint(
            uri: "amqp://guest:guest@localhost:5672",
            endpointName: "myendpoint",
            durableMessages: true,
            createExchanges: true);

        #endregion

        #region rabbit-create-queues-shared-usage

        CreateQueue(
            uri: "amqp://guest:guest@localhost:5672",
            queueName: "error",
            durableMessages: true,
            createExchange: true);

        CreateQueue(
            uri: "amqp://guest:guest@localhost:5672",
            queueName: "audit",
            durableMessages: true,
            createExchange: true);

        #endregion
    }

    #region rabbit-create-queues

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

    #endregion
}
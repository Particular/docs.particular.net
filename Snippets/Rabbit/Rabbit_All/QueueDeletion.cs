using System;
using global::RabbitMQ.Client;

public static class QueueDeletion
{

    static void Usage()
    {
        #region rabbit-delete-queues-endpoint-usage

        DeleteQueuesForEndpoint(
            uri: "amqp://guest:guest@localhost:5672",
            endpointName: "myendpoint");

        #endregion

        #region rabbit-delete-queues-shared-usage

        DeleteQueue(
            uri: "amqp://guest:guest@localhost:5672",
            queueName: "error");
        DeleteQueue(
            uri: "amqp://guest:guest@localhost:5672",
            queueName: "audit");

        #endregion
    }

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
#endregion

}

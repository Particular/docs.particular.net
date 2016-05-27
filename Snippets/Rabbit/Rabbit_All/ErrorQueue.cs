using System;
using System.Linq;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public static class ErrorQueue
{
    public static void ReturnMessageToSourceQueueUsage(ConnectionFactory connectionFactory)
    {
        #region rabbit-return-to-source-queue-usage

        using (var brokerConnection = connectionFactory.CreateConnection())
        {
            ReturnMessageToSourceQueue(
                brokerConnection: brokerConnection,
                errorQueueName: "error",
                messageId: "6698f196-bd50-4f3c-8819-a49e0163d57b");
        }

        #endregion
    }

    #region rabbit-return-to-source-queue

    public static void ReturnMessageToSourceQueue(IConnection brokerConnection, string errorQueueName, string messageId)
    {
        using (var model = brokerConnection.CreateModel())
        {
            BasicGetResult getResult = null;

            do
            {
                getResult = model.BasicGet(errorQueueName, false);

                if (getResult?.BasicProperties.MessageId == messageId)
                {
                    string failedQueueName;
                    ReadFailedQueueHeader(out failedQueueName, getResult);

                    model.BasicPublish(string.Empty, failedQueueName, false, getResult.BasicProperties, getResult.Body);
                    model.BasicAck(getResult.DeliveryTag, false);

                    return;
                }
            } while (getResult != null);

            throw new Exception($"Could not find message with id '{messageId}'");
        }
    }

    static void ReadFailedQueueHeader(out string queueName, BasicGetResult getResult)
    {
        var headerBytes = (byte[])getResult.BasicProperties.Headers["NServiceBus.FailedQ"];
        var header = Encoding.UTF8.GetString(headerBytes);
        // in Version 5 and below the machine name will be included after the @
        // in Version 6 and above it will only be the queue name
        queueName = header.Split('@').First();
    }

    #endregion
}


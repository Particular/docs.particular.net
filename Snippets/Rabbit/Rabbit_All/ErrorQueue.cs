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
            var consumer = new EventingBasicConsumer(model);
            BasicDeliverEventArgs deliverArgs = null;
            var resetEvent = new ManualResetEvent(false);
            consumer.Received += (sender, args) =>
            {
                // already received
                if (deliverArgs != null)
                {
                    return;
                }
                // filter based on specific id
                if (args.BasicProperties.MessageId == messageId)
                {
                    deliverArgs = args;
                    resetEvent.Set();
                }
            };
            model.BasicConsume(errorQueueName, false, Environment.UserName, consumer);
            if (!resetEvent.WaitOne(TimeSpan.FromSeconds(10)))
            {
                throw new Exception($"Could not find message with id '{messageId}' within 10 seconds.");
            }

            string failedQueueName;
            ReadFailedQueueHeader(out failedQueueName, deliverArgs);

            model.BasicPublish(string.Empty, failedQueueName, false, deliverArgs.BasicProperties, deliverArgs.Body);
            model.BasicAck(deliverArgs.DeliveryTag, true);
        }
    }

    static void ReadFailedQueueHeader(out string queueName, BasicDeliverEventArgs deliverArgs)
    {
        var headerBytes = (byte[]) deliverArgs.BasicProperties.Headers["NServiceBus.FailedQ"];
        var header = Encoding.UTF8.GetString(headerBytes);
        // in Version 5 and below the machine name will be included after the @
        // in Version 6 and above it will only be the queue name
        queueName = header.Split('@').First();
    }

    #endregion
}


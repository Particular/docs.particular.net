using System;
using System.Linq;
using System.Text;
using RabbitMQ.Client;

namespace Rabbit_All.ErrorQueue
{
    public static class ErrorQueue
    {
        public static void ReturnMessageToSourceQueueUsage(ConnectionFactory connectionFactory)
        {
            #region rabbit-return-to-source-queue-usage

            using (var connection = connectionFactory.CreateConnection())
            {
                ReturnMessageToSourceQueue(
                    connection: connection,
                    errorQueueName: "error",
                    messageId: "6698f196-bd50-4f3c-8819-a49e0163d57b");
            }

            #endregion
        }

        #region rabbit-return-to-source-queue

        public static void ReturnMessageToSourceQueue(IConnection connection, string errorQueueName, string messageId)
        {
            using (var channel = connection.CreateModel())
            {
                BasicGetResult result;

                do
                {
                    result = channel.BasicGet(errorQueueName, false);

                    if (result == null || result.BasicProperties.MessageId != messageId)
                    {
                        continue;
                    }
                    ReadFailedQueueHeader(out var failedQueueName, result);

                    channel.BasicPublish(string.Empty, failedQueueName, false, result.BasicProperties, result.Body);
                    channel.BasicAck(result.DeliveryTag, false);

                    return;
                } while (result != null);

                throw new Exception($"Could not find message with id '{messageId}'");
            }
        }

        static void ReadFailedQueueHeader(out string queueName, BasicGetResult getResult)
        {
            var headerBytes = (byte[])getResult.BasicProperties.Headers["NServiceBus.FailedQ"];
            var header = Encoding.UTF8.GetString(headerBytes);
            // In Versions 3.3.1 and below the machine name will be included after the @
            // In Versions 3.3.2 and above it will only be the queue name
            queueName = header.Split('@').First();
        }

        #endregion
    }
}


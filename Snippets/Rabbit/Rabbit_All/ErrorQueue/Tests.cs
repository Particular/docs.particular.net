using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing;

namespace Rabbit_All.ErrorQueue
{
    [TestFixture]
    [Explicit]
    public class Tests
    {
        [Test]
        public void ReturnMessageToSourceQueue()
        {
            var sourceQueue = "return-message-to-source-queue";
            var errorQueue = "return-message-to-source-queue-error";

            var connectionFactory = new ConnectionFactory
            {
                HostName = "localhost",
                Port = AmqpTcpEndpoint.UseDefaultPort,
                UserName = "guest",
                Password = "guest",
            };

            var messageId = Guid.NewGuid().ToString();

            using (var connection = connectionFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(sourceQueue, true, false, false, null);
                    channel.QueueDeclare(errorQueue, true, false, false, null);

                    channel.BasicPublish(
                        string.Empty,
                        errorQueue,
                        false,
                        new BasicProperties
                        {
                            MessageId = messageId,
                            Headers = new Dictionary<string, object>
                            {
                                { "NServiceBus.FailedQ", Encoding.UTF8.GetBytes(sourceQueue) },
                            },
                        },
                        new byte[0]);
                }

                ErrorQueue.ReturnMessageToSourceQueue(
                    connection: connection,
                    errorQueueName: errorQueue,
                    messageId: messageId);
            }
        }
    }
}
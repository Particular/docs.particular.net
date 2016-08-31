using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;

namespace Rabbit_All.NativeSend
{
    public static class NativeSend
    {

        static void Usage()
        {
            #region rabbit-nativesend-usage

            SendMessage(
                machineName: "MachineName",
                queueName: "QueueName",
                userName: "admin",
                password: "password",
                messageBody: "{\"Property\":\"PropertyValue\"}",
                headers: new Dictionary<string, object>
                {
                    {
                        "NServiceBus.EnclosedMessageTypes",
                        "MyNamespace.MyMessage"
                    }
                });

            #endregion
        }

        #region rabbit-nativesend

        public static void SendMessage(string machineName, string queueName, string userName, string password, string messageBody, Dictionary<string, object> headers)
        {
            using (var connection = OpenConnection(machineName, userName, password))
            using (var channel = connection.CreateModel())
            {
                var properties = channel.CreateBasicProperties();
                properties.MessageId = Guid.NewGuid().ToString();
                properties.Headers = headers;
                channel.BasicPublish(string.Empty, queueName, false, properties, Encoding.UTF8.GetBytes(messageBody));
            }
        }

        static IConnection OpenConnection(string machine, string userName, string password)
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = machine,
                Port = AmqpTcpEndpoint.UseDefaultPort,
                UserName = userName,
                Password = password,
            };

            return connectionFactory.CreateConnection();
        }

        #endregion
    }
}

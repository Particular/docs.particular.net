namespace Rabbit_All
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using global::RabbitMQ.Client;

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
            using (IModel channel = OpenConnection(machineName, userName, password))
            {
                IBasicProperties properties = channel.CreateBasicProperties();
                properties.MessageId = Guid.NewGuid().ToString();
                properties.Headers = headers;
                channel.BasicPublish(string.Empty, queueName, false, properties, Encoding.UTF8.GetBytes(messageBody));
            }
        }

        static IModel OpenConnection(string machine, string userName, string password)
        {
            ConnectionFactory factory = new ConnectionFactory
            {
                HostName = machine,
                Port = AmqpTcpEndpoint.UseDefaultPort,
                UserName = userName,
                Password = password,
            };
            IConnection conn = factory.CreateConnection();
            return conn.CreateModel();
        }

        #endregion
    }

}
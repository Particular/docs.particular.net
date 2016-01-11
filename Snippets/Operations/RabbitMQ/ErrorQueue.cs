namespace Operations.RabbitMQ
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using global::RabbitMQ.Client;
    using global::RabbitMQ.Client.Events;

    public static class ErrorQueue
    {

        public static void ReturnMessageToSourceQueueUsage()
        {
            #region rabbit-return-to-source-queue-usage

            ReturnMessageToSourceQueue(
                errorQueueMachine: Environment.MachineName,
                errorQueueName: "error",
                userName: "admin",
                password: "password",
                messageId: @"6698f196-bd50-4f3c-8819-a49e0163d57b");

            #endregion
        }

        #region rabbit-return-to-source-queue

        public static void ReturnMessageToSourceQueue(string errorQueueMachine, string errorQueueName, string userName, string password, string messageId)
        {
            using (IModel errorConnection = OpenConnection(errorQueueMachine, userName, password))
            {
                Dictionary<string, object> query = new Dictionary<string, object>
                {
                    {"message_id", messageId}
                };
                QueueingBasicConsumer consumer = new QueueingBasicConsumer(errorConnection);
                string basicConsume = errorConnection.BasicConsume(errorQueueName, false, Environment.UserName, false, true, query, consumer);

                BasicDeliverEventArgs deliverArgs = consumer.Queue.Dequeue();

                string failedQueueName;
                string failedMachineName;
                ReadFailedQueueHeader(out failedQueueName, deliverArgs, out failedMachineName);

                using (IModel targetConnection = OpenConnection(failedMachineName, userName, password))
                {
                    targetConnection.BasicPublish(string.Empty, failedQueueName, true, false, deliverArgs.BasicProperties, deliverArgs.Body);
                }
                errorConnection.BasicAck(deliverArgs.DeliveryTag, true);
            }
        }

        static void ReadFailedQueueHeader(out string queueName, BasicDeliverEventArgs deliverArgs, out string machineName)
        {
            byte[] headerBytes = (byte[]) deliverArgs.BasicProperties.Headers["NServiceBus.FailedQ"];
            string header = Encoding.UTF8.GetString(headerBytes);
            queueName = header.Split('@')[0];
            machineName = header.Split('@')[1];
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
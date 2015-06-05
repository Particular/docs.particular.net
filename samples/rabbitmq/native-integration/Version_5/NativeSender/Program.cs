namespace NativeSender
{
    using System;
    using System.Text;
    using RabbitMQ.Client;

    class Program
    {
        static void Main()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory();

            using (IConnection connection = connectionFactory.CreateConnection())
            {
                Console.Out.WriteLine("Press any key to send a message. Press `q` to quit");

                while (Console.ReadKey().ToString() != "q")
                {
                    using (IModel channel = connection.CreateModel())
                    {
                        IBasicProperties properties = channel.CreateBasicProperties();
                        #region GenerateUniqueMessageId
                        string messageId = Guid.NewGuid().ToString();

                        properties.MessageId = messageId;

                        #endregion

                        #region CreateNativePayload
                        string payload = @"<MyMessage><SomeProperty>Hello from native sender</SomeProperty></MyMessage>";
                        #endregion

                        #region SendMessage

                        channel.BasicPublish(string.Empty, "Samples.RabbitMQ.NativeIntegration", true, false, properties, Encoding.UTF8.GetBytes(payload));
                        #endregion
                        Console.Out.WriteLine("Message with id {0} sent to queue {1}", messageId, "Samples.RabbitMQ.NativeIntegration");
                    }
                }
            }
        }
    }
}

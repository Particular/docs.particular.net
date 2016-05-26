using System;
using System.Text;
using RabbitMQ.Client;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.RabbitMQ.NativeIntegration.Sender";
        ConnectionFactory connectionFactory = new ConnectionFactory();

        using (IConnection connection = connectionFactory.CreateConnection(Console.Title))
        {
            Console.WriteLine("Press enter to send a message");
            Console.WriteLine("Press any key to exit");

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                Console.WriteLine();
                if (key.Key != ConsoleKey.Enter)
                {
                    return;
                }
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

                    channel.BasicPublish(string.Empty, "Samples.RabbitMQ.NativeIntegration", false, properties, Encoding.UTF8.GetBytes(payload));
                    #endregion
                    Console.WriteLine("Message with id {0} sent to queue {1}", messageId, "Samples.RabbitMQ.NativeIntegration");
                }
            }
        }
    }
}
using System;
using System.Text;
using RabbitMQ.Client;

class Program
{
    static void Main()
    {
        var endpointName = "Samples.RabbitMQ.NativeIntegration.Sender";
        
        Console.Title = endpointName;
        
        var connectionFactory = new ConnectionFactory();

        using (var connection = connectionFactory.CreateConnection(endpointName))
        {
            Console.WriteLine("Press enter to send a message");
            Console.WriteLine("Press any key to exit");

            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();
                if (key.Key != ConsoleKey.Enter)
                {
                    return;
                }
                using (var channel = connection.CreateModel())
                {
                    var properties = channel.CreateBasicProperties();

                    //not required but shows how to access basic properties in the receiving NServiceBus endpoint
                    properties.AppId = "MyNativeApp";

                    #region GenerateUniqueMessageId
                    var messageId = Guid.NewGuid().ToString();

                    properties.MessageId = messageId;

                    #endregion

                    #region CreateNativePayload
                    var payload = "<MyMessage><SomeProperty>Hello from native sender</SomeProperty></MyMessage>";
                    #endregion

                    #region SendMessage

                    channel.BasicPublish(
                        exchange: string.Empty,
                        routingKey: "Samples.RabbitMQ.NativeIntegration",
                        mandatory: false,
                        basicProperties: properties,
                        body: Encoding.UTF8.GetBytes(payload));
                    #endregion

                    Console.WriteLine($"Message with id {messageId} sent to queue Samples.RabbitMQ.NativeIntegration");
                }
            }
        }
    }
}

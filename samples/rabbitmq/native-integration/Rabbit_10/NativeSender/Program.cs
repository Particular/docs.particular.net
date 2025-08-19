using System.Text;
using RabbitMQ.Client;

var endpointName = "Samples.RabbitMQ.NativeIntegration.Sender";
Console.Title = endpointName;

var connectionFactory = new ConnectionFactory();

using var connection = await connectionFactory.CreateConnectionAsync(endpointName);
using var channel = await connection.CreateChannelAsync();

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

    var properties = new BasicProperties
    {
        //not required but shows how to access basic properties in the receiving NServiceBus endpoint
        AppId = "MyNativeApp",
        #region GenerateUniqueMessageId
        MessageId = Guid.NewGuid().ToString()
        #endregion
    };

    #region CreateNativePayload
    var payload = "<MyMessage><SomeProperty>Hello from native sender</SomeProperty></MyMessage>";
    #endregion

    #region SendMessage
    await channel.BasicPublishAsync(
        exchange: string.Empty,
        routingKey: "Samples.RabbitMQ.NativeIntegration",
        mandatory: false,
        basicProperties: properties,
        body: Encoding.UTF8.GetBytes(payload));
    #endregion

    Console.WriteLine($"Message with id {properties.MessageId} sent to queue Samples.RabbitMQ.NativeIntegration");
}

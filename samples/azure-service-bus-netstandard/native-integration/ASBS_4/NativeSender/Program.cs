using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

Console.Title = "Sender";
var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus_ConnectionString");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new Exception("Could not read the 'AzureServiceBus_ConnectionString' environment variable. Check the sample prerequisites.");
}

var serviceBusClient = new ServiceBusClient(connectionString);
var serviceBusSender = serviceBusClient.CreateSender("Samples.ASB.NativeIntegration");

#region SerializedMessage

var nativeMessage = new NativeMessage
{
    Content = "Hello from native sender",
    SentOnUtc = DateTime.UtcNow
};

var json = JsonSerializer.Serialize(nativeMessage);

#endregion

var bytes = Encoding.UTF8.GetBytes(json);

var message = new ServiceBusMessage(bytes)
{
    MessageId = Guid.NewGuid().ToString(),

    ApplicationProperties =
    {
        #region NecessaryHeaders
        ["NServiceBus.EnclosedMessageTypes"] = typeof(NativeMessage).FullName
        #endregion
    }
};

await serviceBusSender.SendMessageAsync(message);

Console.WriteLine($"Native message sent on {nativeMessage.SentOnUtc} UTC");
Console.WriteLine("Press any key to exit");
Console.ReadKey();
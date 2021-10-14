using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.ASB.NativeIntegration.Sender";
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

        var json = JsonConvert.SerializeObject(nativeMessage);

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

        await serviceBusSender.SendMessageAsync(message)
            .ConfigureAwait(false);

        Console.WriteLine("Native message sent");
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
    }
}
using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

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

        var queueClient = new QueueClient(connectionString, "Samples.ASB.NativeIntegration");

        #region SerializedMessage

        var nativeMessage = new NativeMessage
        {
            Content = "Hello from native sender",
            SentOnUtc = DateTime.UtcNow
        };

        var json = JsonConvert.SerializeObject(nativeMessage);

        #endregion

        var bytes = Encoding.UTF8.GetBytes(json);

        var message = new Message(bytes)
        {
            MessageId = Guid.NewGuid().ToString(),

            UserProperties =
            {
                #region NecessaryHeaders
                ["NServiceBus.EnclosedMessageTypes"] = typeof(NativeMessage).FullName
                #endregion
            }
        };

        await queueClient.SendAsync(message)
            .ConfigureAwait(false);

        Console.WriteLine("Native message sent");
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
    }
}
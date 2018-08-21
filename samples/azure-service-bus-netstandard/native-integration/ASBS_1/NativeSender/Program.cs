using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.ASB.NativeIntegration.Sender";
        var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus_ConnectionString");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read the 'AzureServiceBus.ConnectionString' environment variable. Check the sample prerequisites.");
        }

        var queueClient = new QueueClient(connectionString, "Samples.ASB.NativeIntegration");

        #region SerializedMessage

        var nativeMessage = @"{""Content"":""Hello from native sender"",""SendOnUtc"":""2018-08-21T20:55:52.6448469Z""}";

        #endregion

        var nativeMessageAsBytes = Encoding.UTF8.GetBytes(nativeMessage);

        var message = new Message(nativeMessageAsBytes)
        {
            MessageId = Guid.NewGuid().ToString(),

            UserProperties =
            {
                #region NecessaryHeaders
                ["NServiceBus.EnclosedMessageTypes"] = "NativeMessage",
                // Required to support ServiceControl that is using ASB v6.x
                ["NServiceBus.MessageIntent"] = "Send"
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
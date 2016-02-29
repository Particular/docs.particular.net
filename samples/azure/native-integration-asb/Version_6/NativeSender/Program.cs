using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;

class Program
{
    static void Main()
    {
        MainAsync().GetAwaiter().GetResult();
    }

    static async Task MainAsync()
    {
        Console.Title = "Samples.ASB.NativeIntegration.Sender";
        string connectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString");
        QueueClient queueClient = QueueClient.CreateFromConnectionString(connectionString, "Samples.ASB.NativeIntegration");

        #region SerializedMessage

        string nativeMessage = @"{""Content"":""Hello from native sender"",""SendOnUtc"":""2015-10-27T20:47:27.4682716Z""}";

        #endregion

        MemoryStream nativeMessageAsStream = new MemoryStream(Encoding.UTF8.GetBytes(nativeMessage));

        BrokeredMessage message = new BrokeredMessage(nativeMessageAsStream)
        {
            MessageId = Guid.NewGuid().ToString()
        };

        #region NecessaryHeaders

        message.Properties["NServiceBus.EnclosedMessageTypes"] = "NativeMessage";
        message.Properties["NServiceBus.MessageIntent"] = "Send";
        message.Properties["Transport-Encoding"] = "application/octect-stream";

        #endregion

        await queueClient.SendAsync(message);

        Console.WriteLine("Native message sent");
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
    }
}

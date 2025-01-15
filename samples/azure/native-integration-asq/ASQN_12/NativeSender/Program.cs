using System;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Newtonsoft.Json;

class Program
{
    static async Task Main()
    {
        var queueClient = new QueueClient("UseDevelopmentStorage=true", "native-integration-asq");

        await queueClient.CreateIfNotExistsAsync();

        Console.WriteLine("Press Enter to send a native message.");
        Console.WriteLine("Press any other key to exit.");
        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            if (key.Key != ConsoleKey.Enter)
            {
                break;
            }

            #region send-a-native-message

            var nativeMessage = new NativeMessage
            {
                NativeMessageId = Guid.NewGuid(),
                Content = $"Hello from native sender @ {DateTimeOffset.Now}"
            };

            var serializedMessage = JsonConvert.SerializeObject(nativeMessage);

            await queueClient.SendMessageAsync(serializedMessage);

            #endregion

            Console.WriteLine("Message sent");
        }
    }
}

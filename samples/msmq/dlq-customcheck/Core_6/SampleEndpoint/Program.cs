using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static void Main(string[] args)
    {
        MainAsync().GetAwaiter().GetResult();
    }

    static async Task MainAsync()
    {
        MsmqUtils.SetUpDummyQueue();

        var config = new EndpointConfiguration("SampleEndpoint");
        config.UseTransport<MsmqTransport>();
        config.UsePersistence<InMemoryPersistence>();
        config.SendFailedMessagesTo("error");

        #region configure-custom-checks

        config.CustomCheckPlugin("Particular.ServiceControl");
        
        #endregion

        var endpoint = await Endpoint.Start(config).ConfigureAwait(false);

        Console.WriteLine("Endpoint Started");
        Console.WriteLine("Press [d] to send a message to the Dead Letter Queue");
        Console.WriteLine("Press any other key to exit");

        while (Console.ReadKey(true).Key == ConsoleKey.D)
        {
            MsmqUtils.SendMessageToDeadLetterQueue(DateTime.UtcNow.ToShortTimeString());
            Console.WriteLine("Sent message to Dead Letter Queue");
        }

        await endpoint.Stop().ConfigureAwait(false);
    }
}
using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static void Main()
    {
        MainAsync().GetAwaiter().GetResult();
    }

    static async Task MainAsync()
    {
        MsmqUtils.SetUpDummyQueue();

        var endpointConfiguration = new EndpointConfiguration("SampleEndpoint");
        endpointConfiguration.UseTransport<MsmqTransport>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");

        #region configure-custom-checks

        endpointConfiguration.CustomCheckPlugin("Particular.ServiceControl");

        #endregion

        var endpoint = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Endpoint Started");
        Console.WriteLine("Press [d] to send a message to the Dead Letter Queue");
        Console.WriteLine("Press any other key to exit");

        while (Console.ReadKey(true).Key == ConsoleKey.D)
        {
            MsmqUtils.SendMessageToDeadLetterQueue(DateTime.UtcNow.ToShortTimeString());
            Console.WriteLine("Sent message to Dead Letter Queue");
        }

        await endpoint.Stop()
            .ConfigureAwait(false);
    }
}
using System;
using NServiceBus;

class Program
{
    static void Main()
    {
        MsmqUtils.SetUpDummyQueue();

        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("SampleEndpoint");
        busConfiguration.UseTransport<MsmqTransport>();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Endpoint Started");
            Console.WriteLine("Press [d] to send a message to the Dead Letter Queue");
            Console.WriteLine("Press any other key to exit");

            while (Console.ReadKey(true).Key == ConsoleKey.D)
            {
                MsmqUtils.SendMessageToDeadLetterQueue(DateTime.UtcNow.ToShortTimeString());
                Console.WriteLine("Sent message to Dead Letter Queue");
            }
        }
    }
}
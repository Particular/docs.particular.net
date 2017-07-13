using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.PipelineHandlerTimer";
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.PipelineHandlerTimer");
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();
        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Run(bus);
        }
    }

    static void Run(IBus bus)
    {
        Console.WriteLine("Press 'Enter' to send a Message");
        Console.WriteLine("Press any key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            if (key.Key == ConsoleKey.Enter)
            {
                SendMessage(bus);
                continue;
            }
            return;
        }
    }

    static void SendMessage(IBus bus)
    {
        var message = new Message();
        bus.SendLocal(message);

        Console.WriteLine();
        Console.WriteLine("Message sent");
    }

}
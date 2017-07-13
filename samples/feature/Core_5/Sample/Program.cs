using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.Features";
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Features");
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();
        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Run(bus);
        }
    }

    static void Run(IBus bus)
    {
        Console.WriteLine("Press 'H' to send a HandlerMessage");
        Console.WriteLine("Press 'S' to send a StartSagaMessage");
        Console.WriteLine("Press any other key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            if (key.Key == ConsoleKey.H)
            {
                SendHandlerMessage(bus);
                continue;
            }
            if (key.Key == ConsoleKey.S)
            {
                SendSagaMessage(bus);
                continue;
            }
            return;
        }
    }

    static void SendHandlerMessage(IBus bus)
    {
        var message = new HandlerMessage();
        bus.SendLocal(message);

        Console.WriteLine();
        Console.WriteLine("HandlerMessage sent");
    }
    static void SendSagaMessage(IBus bus)
    {
        var message = new StartSagaMessage
        {
            SentTime = DateTimeOffset.UtcNow,
            TheId = Guid.NewGuid()
        };
        bus.SendLocal(message);

        Console.WriteLine();
        Console.WriteLine("StartSagaMessage sent");
    }

}
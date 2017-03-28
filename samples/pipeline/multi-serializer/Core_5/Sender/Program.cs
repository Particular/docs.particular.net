using System;
using NServiceBus;
using NServiceBus.Logging;

class Program
{

    static void Main()
    {
        var defaultFactory = LogManager.Use<DefaultFactory>();
        defaultFactory.Level(LogLevel.Info);

        Console.Title = "Samples.MultiSerializer.Sender";
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.MultiSerializer.Sender");
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();
        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Run(bus);
        }
    }


    static void Run(IBus bus)
    {
        Console.WriteLine("Press 'J' to send a JSON message");
        Console.WriteLine("Press 'X' to send a XML message");
        Console.WriteLine("Press any key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            if (key.Key == ConsoleKey.X)
            {
                SendXmlMessage(bus);
                continue;
            }
            if (key.Key == ConsoleKey.J)
            {
                SendJsonMessage(bus);
                continue;
            }
            return;
        }
    }

    static void SendXmlMessage(IBus bus)
    {
        var message = new MessageWithXml
        {
            SomeProperty = "Some content in a xml message",
        };
        bus.Send("Samples.MultiSerializer.Receiver", message);
        Console.WriteLine("Xml message sent");
    }

    static void SendJsonMessage(IBus bus)
    {
        var message = new MessageWithJson
        {
            SomeProperty = "Some content in a json message",
        };
        bus.Send("Samples.MultiSerializer.Receiver", message);
        Console.WriteLine("Json Message sent");
    }
}
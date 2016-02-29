using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.MultiSerializer.Sender";
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.MultiSerializer.Sender");
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();
        using (IBus bus = Bus.Create(busConfiguration).Start())
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
            ConsoleKeyInfo key = Console.ReadKey();
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
        MessageWithXml message = new MessageWithXml
        {
            SomeProperty = "Some content in a xml message",
        };
        bus.Send("Samples.MultiSerializer.Receiver", message);
        Console.WriteLine("Xml message sent");
    }

    static void SendJsonMessage(IBus bus)
    {
        MessageWithJson message = new MessageWithJson
        {
            SomeProperty = "Some content in a json message",
        };
        bus.Send("Samples.MultiSerializer.Receiver", message);
        Console.WriteLine("Json Message sent");
    }
}
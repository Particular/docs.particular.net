using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Sample.MultiSerializer.Sender");
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
        Console.WriteLine("Press 'B' to send a Binary message");
        Console.WriteLine("To exit, press Ctrl + C");

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey();

            if (key.Key == ConsoleKey.B)
            {
                SendBinaryMessage(bus);
                continue;
            }
            if (key.Key == ConsoleKey.J)
            {
                SendJsonMessage(bus);
            }
        }
    }

    static void SendBinaryMessage(IBus bus)
    {
        MessageWithBinary message = new MessageWithBinary
        {
            SomeProperty = "Some content in a binary message",
        };
        bus.Send("Sample.MultiSerializer.Receiver", message);
        Console.WriteLine();
        Console.WriteLine("Binary message sent");
    }

    static void SendJsonMessage(IBus bus)
    {
        MessageWithJson message = new MessageWithJson
                                  {
                                      SomeProperty = "Some content in a json message",
                                  };
        bus.Send("Sample.MultiSerializer.Receiver", message);
        Console.WriteLine();
        Console.WriteLine("Json Message sent");
    }
}


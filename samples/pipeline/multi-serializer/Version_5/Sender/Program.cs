using System;
using NServiceBus;

class Program
{
    static IStartableBus bus;

    static void Main()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Sample.MultiSerializer.Sender");
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();
        bus = Bus.Create(busConfiguration);
        bus.Start();
        Run();
    }


    static void Run()
    {
        Console.WriteLine("Press 'J' to send a JSON message");
        Console.WriteLine("Press 'B' to send a Binary message");
        Console.WriteLine("To exit, press Ctrl + C");

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey();

            if (key.Key == ConsoleKey.B)
            {
                SendBinaryMessage();
                continue;
            }
            if (key.Key == ConsoleKey.J)
            {
                SendJsonMessage();
            }
        }
    }

    static void SendBinaryMessage()
    {
        MessageWithBinary message = new MessageWithBinary
        {
            SomeProperty = "Some content in a binary message",
        };
        bus.Send("Sample.MultiSerializer.Receiver", message);
        Console.WriteLine();
        Console.WriteLine("Binary message sent");
    }

    static void SendJsonMessage()
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


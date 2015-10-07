using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.MultiSerializer.Sender");
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();
        busConfiguration.SendFailedMessagesTo("error");
        using (IBus bus = await Bus.Create(busConfiguration).StartAsync())
        {
            await Run(bus);
        }
    }


    static async Task Run(IBus bus)
    {
        Console.WriteLine("Press 'J' to send a JSON message");
        Console.WriteLine("Press 'B' to send a Binary message");
        Console.WriteLine("Press any key to exit");

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey();
            Console.WriteLine();

            if (key.Key == ConsoleKey.B)
            {
                await SendBinaryMessage(bus);
                continue;
            }
            if (key.Key == ConsoleKey.J)
            {
                await SendJsonMessage(bus);
                continue;
            }
            break;
        }
    }

    static async Task SendBinaryMessage(IBus bus)
    {
        MessageWithBinary message = new MessageWithBinary
        {
            SomeProperty = "Some content in a binary message",
        };
        await bus.SendAsync("Samples.MultiSerializer.Receiver", message);
        Console.WriteLine("Binary message sent");
    }

    static async Task SendJsonMessage(IBus bus)
    {
        MessageWithJson message = new MessageWithJson
        {
            SomeProperty = "Some content in a json message",
        };
        await bus.SendAsync("Samples.MultiSerializer.Receiver", message);
        Console.WriteLine("Json Message sent");
    }
}
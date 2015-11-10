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
        Console.WriteLine("Press 'X' to send a XML message");
        Console.WriteLine("Press any key to exit");

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey();
            Console.WriteLine();

            if (key.Key == ConsoleKey.X)
            {
                await SendXmlMessage(bus);
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

    static async Task SendXmlMessage(IBus bus)
    {
        MessageWithXml message = new MessageWithXml
        {
            SomeProperty = "Some content in a Xml message",
        };
        await bus.SendAsync("Samples.MultiSerializer.Receiver", message);
        Console.WriteLine("XML message sent");
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
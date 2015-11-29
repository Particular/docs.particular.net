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

        IEndpointInstance endpoint = await Endpoint.Start(busConfiguration);
        try
        {
            IBusContext busContext = endpoint.CreateBusContext();
            await Run(busContext);
        }
        finally
        {
            await endpoint.Stop();
        }
    }


    static async Task Run(IBusContext busContext)
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
                await SendXmlMessage(busContext);
                continue;
            }
            if (key.Key == ConsoleKey.J)
            {
                await SendJsonMessage(busContext);
                continue;
            }
            break;
        }
    }

    static async Task SendXmlMessage(IBusContext busContext)
    {
        MessageWithXml message = new MessageWithXml
        {
            SomeProperty = "Some content in a Xml message",
        };
        await busContext.Send("Samples.MultiSerializer.Receiver", message);
        Console.WriteLine("XML message sent");
    }

    static async Task SendJsonMessage(IBusContext busContext)
    {
        MessageWithJson message = new MessageWithJson
        {
            SomeProperty = "Some content in a json message",
        };
        await busContext.Send("Samples.MultiSerializer.Receiver", message);
        Console.WriteLine("Json Message sent");
    }
}
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
            IBusSession busSession = endpoint.CreateBusSession();
            await Run(busSession);
        }
        finally
        {
            await endpoint.Stop();
        }
    }


    static async Task Run(IBusSession busSession)
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
                await SendXmlMessage(busSession);
                continue;
            }
            if (key.Key == ConsoleKey.J)
            {
                await SendJsonMessage(busSession);
                continue;
            }
            break;
        }
    }

    static async Task SendXmlMessage(IBusSession busSession)
    {
        MessageWithXml message = new MessageWithXml
        {
            SomeProperty = "Some content in a Xml message",
        };
        await busSession.Send("Samples.MultiSerializer.Receiver", message);
        Console.WriteLine("XML message sent");
    }

    static async Task SendJsonMessage(IBusSession busSession)
    {
        MessageWithJson message = new MessageWithJson
        {
            SomeProperty = "Some content in a json message",
        };
        await busSession.Send("Samples.MultiSerializer.Receiver", message);
        Console.WriteLine("Json Message sent");
    }
}
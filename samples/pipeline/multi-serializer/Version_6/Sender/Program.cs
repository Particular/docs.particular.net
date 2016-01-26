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
            await Run(endpoint);
        }
        finally
        {
            await endpoint.Stop();
        }
    }


    static async Task Run(IEndpointInstance endpointInstance)
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
                await SendXmlMessage(endpointInstance);
                continue;
            }
            if (key.Key == ConsoleKey.J)
            {
                await SendJsonMessage(endpointInstance);
                continue;
            }
            break;
        }
    }

    static async Task SendXmlMessage(IEndpointInstance endpointInstance)
    {
        MessageWithXml message = new MessageWithXml
        {
            SomeProperty = "Some content in a Xml message",
        };
        await endpointInstance.Send("Samples.MultiSerializer.Receiver", message);
        Console.WriteLine("XML message sent");
    }

    static async Task SendJsonMessage(IEndpointInstance endpointInstance)
    {
        MessageWithJson message = new MessageWithJson
        {
            SomeProperty = "Some content in a json message",
        };
        await endpointInstance.Send("Samples.MultiSerializer.Receiver", message);
        Console.WriteLine("Json Message sent");
    }
}
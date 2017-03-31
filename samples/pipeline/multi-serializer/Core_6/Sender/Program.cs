using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        var defaultFactory = LogManager.Use<DefaultFactory>();
        defaultFactory.Level(LogLevel.Info);

        Console.Title = "Samples.MultiSerializer.Sender";
        var endpointConfiguration = new EndpointConfiguration("Samples.MultiSerializer.Sender");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        await Run(endpointInstance)
            .ConfigureAwait(false);
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }

    static async Task Run(IEndpointInstance endpointInstance)
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
                await SendXmlMessage(endpointInstance)
                    .ConfigureAwait(false);
                continue;
            }
            if (key.Key == ConsoleKey.J)
            {
                await SendJsonMessage(endpointInstance)
                    .ConfigureAwait(false);
                continue;
            }
            break;
        }
    }

    static async Task SendXmlMessage(IEndpointInstance endpointInstance)
    {
        var message = new MessageWithXml
        {
            SomeProperty = "Some content in a Xml message",
        };
        await endpointInstance.Send("Samples.MultiSerializer.Receiver", message)
            .ConfigureAwait(false);
        Console.WriteLine("XML message sent");
    }

    static async Task SendJsonMessage(IEndpointInstance endpointInstance)
    {
        var message = new MessageWithJson
        {
            SomeProperty = "Some content in a json message",
        };
        await endpointInstance.Send("Samples.MultiSerializer.Receiver", message)
            .ConfigureAwait(false);
        Console.WriteLine("Json Message sent");
    }
}
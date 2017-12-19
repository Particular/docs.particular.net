using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main(string[] args)
    {
        Console.Title = "Samples.SessionFilter.Sender";
        var endpointConfiguration = new EndpointConfiguration("Samples.SessionFilter.Sender");

        endpointConfiguration.UsePersistence<LearningPersistence>();
        var transport = endpointConfiguration.UseTransport<LearningTransport>();

        transport.Routing().RouteToEndpoint(
            typeof(SomeMessage), 
            "Samples.SessionFilter.Receiver"
        );

        #region add-filter-behavior

        var sessionKeyProvider = new RotatingSessionKeyProvider();

        endpointConfiguration.ApplySessionFilter(sessionKeyProvider);

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        #endregion

        await MainLoop(endpointInstance, sessionKeyProvider)
            .ConfigureAwait(false);

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }

    static async Task MainLoop(IEndpointInstance endpoint, RotatingSessionKeyProvider sessionKeyProvider)
    {
        PrintMenu(sessionKeyProvider);

        var index = 1;

        while (true)
        {

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.C:
                    sessionKeyProvider.NextKey();
                    PrintMenu(sessionKeyProvider);
                    break;
                case ConsoleKey.Escape:
                    return;
                default:
                    await endpoint.Send(new SomeMessage { Counter = index })
                        .ConfigureAwait(false);
                    Console.WriteLine($"Sent message {index++}");
                    break;
            }
        }
    }

    static void PrintMenu(ISessionKeyProvider sessionKeyProvider)
    {
        Console.Clear();
        Console.WriteLine($"Session Key: {sessionKeyProvider.SessionKey}");
        Console.WriteLine("C)   Change Session Key");
        Console.WriteLine("ESC) Close");
        Console.WriteLine("any other key to send a message");
    }
}
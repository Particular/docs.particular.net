using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main(string[] args)
    {
        Console.Title = "Sender";
        var endpointConfiguration = new EndpointConfiguration("Samples.SessionFilter.Sender");

        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        var routing = endpointConfiguration.UseTransport(new LearningTransport());

        routing.RouteToEndpoint(
            typeof(SomeMessage),
            "Samples.SessionFilter.Receiver"
        );

        #region register-session-key-provider

        var sessionKeyProvider = new RotatingSessionKeyProvider();

        endpointConfiguration.ApplySessionFilter(sessionKeyProvider);

        var endpointInstance = await Endpoint.Start(endpointConfiguration);

        #endregion

        await MainLoop(endpointInstance, sessionKeyProvider);

        await endpointInstance.Stop();
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
                    await endpoint.Send(new SomeMessage { Counter = index });
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

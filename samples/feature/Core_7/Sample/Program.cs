using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        //required to prevent possible occurrence of .NET Core issue https://github.com/dotnet/coreclr/issues/12668
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

        Console.Title = "Samples.Features";
        var endpointConfiguration = new EndpointConfiguration("Samples.Features");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        await Run(endpointInstance)
            .ConfigureAwait(false);
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }

    static async Task Run(IEndpointInstance endpointInstance)
    {
        Console.WriteLine("Press 'H' to send a HandlerMessage");
        Console.WriteLine("Press 'S' to send a StartSagaMessage");
        Console.WriteLine("Press any other key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            if (key.Key == ConsoleKey.H)
            {
                await SendHandlerMessage(endpointInstance)
                    .ConfigureAwait(false);
                continue;
            }
            if (key.Key == ConsoleKey.S)
            {
                await SendSagaMessage(endpointInstance)
                    .ConfigureAwait(false);
                continue;
            }
            return;
        }
    }

    static Task SendHandlerMessage(IMessageSession endpointInstance)
    {
        Console.WriteLine();
        Console.WriteLine("HandlerMessage sent");
        var message = new HandlerMessage();
        return endpointInstance.SendLocal(message);
    }

    static Task SendSagaMessage(IMessageSession endpointInstance)
    {
        Console.WriteLine();
        Console.WriteLine("StartSagaMessage sent");
        var message = new StartSagaMessage
        {
            SentTime = DateTimeOffset.UtcNow,
            TheId = Guid.NewGuid()
        };
        return endpointInstance.SendLocal(message);
    }
}
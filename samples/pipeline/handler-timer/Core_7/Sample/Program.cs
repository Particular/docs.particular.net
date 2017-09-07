using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;

class Program
{

    static void Main()
    {
        //required to prevent possible occurrence of .NET Core issue https://github.com/dotnet/coreclr/issues/12668
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

        Start().GetAwaiter().GetResult();
    }

    static async Task Start()
    {
        Console.Title = "Samples.PipelineHandlerTimer";
        var endpointConfiguration = new EndpointConfiguration("Samples.PipelineHandlerTimer");
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
        Console.WriteLine("Press 'Enter' to send a Message");
        Console.WriteLine("Press any key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            if (key.Key == ConsoleKey.Enter)
            {
                await SendMessage(endpointInstance)
                    .ConfigureAwait(false);
                continue;
            }
            return;
        }
    }

    static Task SendMessage(IEndpointInstance endpointInstance)
    {
        Console.WriteLine();
        Console.WriteLine("Message sent");
        var message = new Message();
        return endpointInstance.SendLocal(message);
    }

}
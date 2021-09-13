using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.PipelineHandlerTimer";
        var endpointConfiguration = new EndpointConfiguration("Samples.PipelineHandlerTimer");

        endpointConfiguration.UseTransport(new LearningTransport());

        #region pipeline-config
        endpointConfiguration.Pipeline.Register(typeof(HandlerTimerBehavior), "Logs a warning if a handler take more than a specified time");
        #endregion

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
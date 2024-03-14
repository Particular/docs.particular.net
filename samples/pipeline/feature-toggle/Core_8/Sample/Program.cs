using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.PipelineFeatureToggle";
        var endpointConfiguration = new EndpointConfiguration("Samples.PipelineFeatureToggle");

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport(new LearningTransport());

        #region enable-feature

        var toggles = endpointConfiguration.EnableFeatureToggles();
        toggles.AddToggle(ctx => ctx.MessageHandler.HandlerType == typeof(Handler2));


        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        await Run(endpointInstance);
        await endpointInstance.Stop();
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
                await SendMessage(endpointInstance);
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

using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Pipeline.CrossCuttingConcerns.Endpoint";

        var endpointConfiguration = new EndpointConfiguration("Samples.Pipeline.CrossCuttingConcerns.Endpoint");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        #region configuration
        var pipeline = endpointConfiguration.Pipeline;
        pipeline.Register(new MyAuthBehavior(), "Creates the auth context");
        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        var key = default(ConsoleKeyInfo);

        Console.WriteLine("Press any key to send messages, 'q' to exit");
        while (key.KeyChar != 'q')
        {
            key = Console.ReadKey();

            var options = new SendOptions();
            options.SetHeader("auth_token", Guid.NewGuid().ToString());
            options.SetHeader("auth_login", "john_doe");
            options.RouteToThisEndpoint();
            await endpointInstance.Send(new MyMessage(), options)
                .ConfigureAwait(false);
        }

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}
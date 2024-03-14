using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Pipeline.UnitOfWork.Endpoint";

        var endpointConfiguration = new EndpointConfiguration("Samples.Pipeline.UnitOfWork.Endpoint");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport(new LearningTransport());

        #region configuration
        var sessionProvider = new MySessionProvider();

        var pipeline = endpointConfiguration.Pipeline;
        pipeline.Register(new MyUowBehavior(sessionProvider), "Manages the session");
        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration);

        var key = default(ConsoleKeyInfo);

        Console.WriteLine("Press any key to send messages, 'q' to exit");
        while (key.KeyChar != 'q')
        {
            key = Console.ReadKey();

            var options = new SendOptions();
            options.RouteToThisEndpoint();
            await endpointInstance.Send(new MyMessage(), options);
        }

        await endpointInstance.Stop();
    }
}

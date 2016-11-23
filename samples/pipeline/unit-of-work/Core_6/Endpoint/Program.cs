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
        Console.Title = "Samples.Pipeline.UnitOfWork.Endpoint";

        var endpointConfiguration = new EndpointConfiguration("Samples.Pipeline.UnitOfWork.Endpoint");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        #region configuration
        endpointConfiguration.RegisterComponents(
            registration: components =>
            {
                components.ConfigureComponent(
                    componentFactory: builder => new MySessionProvider(),
                    dependencyLifecycle: DependencyLifecycle.SingleInstance);
            });
        var pipeline = endpointConfiguration.Pipeline;
        pipeline.Register(typeof(MyUowBehavior), "Manages the session");
        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        var key = default(ConsoleKeyInfo);

        Console.WriteLine("Press any key to send messages, 'q' to exit");
        while (key.KeyChar != 'q')
        {
            key = Console.ReadKey();

            for (var i = 1; i < 4; i++)
            {
                var options = new SendOptions();
                options.SetHeader("tennant", "tennant" + i);
                options.RouteToThisEndpoint();
                await endpointInstance.Send(new MyMessage(), options);
            }
        }

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}
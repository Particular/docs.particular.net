using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "UnitOfWorkEndpoint";

        var endpointConfiguration = new EndpointConfiguration("Samples.Pipeline.UnitOfWork.Endpoint");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        #region configuration

        var pipeline = endpointConfiguration.Pipeline;
        pipeline.Register(new MyUowBehavior(), "Manages the session");
        endpointConfiguration.RegisterComponents(c =>
        {
            c.ConfigureComponent<MySession>(DependencyLifecycle.InstancePerUnitOfWork);
        });
        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration);

        var key = default(ConsoleKeyInfo);

        Console.WriteLine("Press any key to send messages, 'q' to exit");
        while (key.KeyChar != 'q')
        {
            key = Console.ReadKey();

            for (var i = 1; i < 4; i++)
            {
                var options = new SendOptions();
                options.SetHeader("tenant", "tenant" + i);
                options.RouteToThisEndpoint();
                await endpointInstance.Send(new MyMessage(), options);
            }
        }

        await endpointInstance.Stop();
    }
}
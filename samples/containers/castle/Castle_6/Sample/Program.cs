using System;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NServiceBus;

static class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.Castle";
        #region ContainerConfiguration

        EndpointConfiguration endpointConfiguration = new EndpointConfiguration("Samples.Castle");

        WindsorContainer container = new WindsorContainer();
        container.Register(Component.For<MyService>().Instance(new MyService()));

        endpointConfiguration.UseContainer<WindsorBuilder>(c => c.ExistingContainer(container));

        #endregion

        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();

        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
        try
        {
            await endpoint.SendLocal(new MyMessage());
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        finally
        {
            await endpoint.Stop();
        }
    }
}
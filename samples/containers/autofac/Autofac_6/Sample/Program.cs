using System;
using System.Threading.Tasks;
using Autofac;
using NServiceBus;

static class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.Autofac";
        #region ContainerConfiguration

        EndpointConfiguration endpointConfiguration = new EndpointConfiguration("Samples.Autofac");

        ContainerBuilder builder = new ContainerBuilder();
        builder.RegisterInstance(new MyService());
        IContainer container = builder.Build();
        endpointConfiguration.UseContainer<AutofacBuilder>(c => c.ExistingLifetimeScope(container));

        #endregion

        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

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
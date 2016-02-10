using System;
using System.Threading.Tasks;
using NServiceBus;
using Spring.Context.Support;

static class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        #region ContainerConfiguration
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
        endpointConfiguration.EndpointName("Samples.Spring");

        GenericApplicationContext applicationContext = new GenericApplicationContext();
        applicationContext.ObjectFactory.RegisterSingleton("MyService", new MyService());
        endpointConfiguration.UseContainer<SpringBuilder>(c => c.ExistingApplicationContext(applicationContext));
        #endregion
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();

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
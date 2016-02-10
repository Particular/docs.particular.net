using System;
using System.Threading.Tasks;
using Ninject;
using NServiceBus;

public class Program
{
    static void Main()
    {
        Start().GetAwaiter().GetResult();
    }

    static async Task Start()
    {
        #region ContainerConfiguration
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Ninject");
        busConfiguration.SendFailedMessagesTo("error");

        StandardKernel kernel = new StandardKernel();
        kernel.Bind<MyService>().ToConstant(new MyService());
        busConfiguration.UseContainer<NinjectBuilder>(c => c.ExistingKernel(kernel));
        #endregion
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();


        IEndpointInstance endpointInstance = await Endpoint.Start(busConfiguration);

        try
        {
            await endpointInstance.SendLocal(new MyMessage());

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        finally 
        {
            await endpointInstance.Stop();
        }
    }
}
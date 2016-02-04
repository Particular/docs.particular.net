using System;
using System.Threading.Tasks;
using Ninject;
using NServiceBus;

public class Program
{
    static void Main()
    {
        new Program().Start().Wait();
    }

    async Task Start()
    {
        #region ContainerConfiguration
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Ninject");

        StandardKernel kernel = new StandardKernel();
        kernel.Bind<MyService>().ToConstant(new MyService());
        busConfiguration.UseContainer<NinjectBuilder>(c => c.ExistingKernel(kernel));
        #endregion
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();


        IEndpointInstance endpointInstance = await Endpoint.Start(busConfiguration);
        await endpointInstance.SendLocal(new MyMessage());

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}
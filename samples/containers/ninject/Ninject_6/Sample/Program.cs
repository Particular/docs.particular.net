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
        Console.Title = "Samples.Ninject";
        #region ContainerConfiguration
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration("Samples.Ninject");
        endpointConfiguration.SendFailedMessagesTo("error");

        StandardKernel kernel = new StandardKernel();
        kernel.Bind<MyService>().ToConstant(new MyService());
        endpointConfiguration.UseContainer<NinjectBuilder>(c => c.ExistingKernel(kernel));
        #endregion
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();


        IEndpointInstance endpointInstance = await Endpoint.Start(endpointConfiguration);

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
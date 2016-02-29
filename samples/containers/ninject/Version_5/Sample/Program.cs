using System;
using Ninject;
using NServiceBus;

static class Program
{
    static void Main()
    {
        Console.Title = "Samples.Ninject";
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

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            bus.SendLocal(new MyMessage());
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
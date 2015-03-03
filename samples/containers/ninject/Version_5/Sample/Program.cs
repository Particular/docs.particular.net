using System;
using Ninject;
using NServiceBus;

static class Program
{
    static void Main()
    {
        #region ContainerConfiguration
        BusConfiguration configuration = new BusConfiguration();
        configuration.EndpointName("Samples.Ninject");

        StandardKernel kernel = new StandardKernel();
        kernel.Bind<MyService>().ToConstant(new MyService());
        configuration.UseContainer<NinjectBuilder>(c => c.ExistingKernel(kernel));
        #endregion
        configuration.UseSerialization<JsonSerializer>();
        configuration.UsePersistence<InMemoryPersistence>();
        configuration.EnableInstallers();

        using (IStartableBus bus = Bus.Create(configuration))
        {
            bus.Start();
            bus.SendLocal(new MyMessage());
            Console.WriteLine("Press any key to exit");
            Console.Read();

        }
    }
}
using System;
using Microsoft.Practices.Unity;
using NServiceBus;

static class Program
{
    static void Main()
    {
        #region ContainerConfiguration
        BusConfiguration configuration = new BusConfiguration();
        configuration.EndpointName("Samples.Unity");

        UnityContainer container = new UnityContainer();
        container.RegisterInstance(new MyService());
        configuration.UseContainer<UnityBuilder>(c => c.UseExistingContainer(container));
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
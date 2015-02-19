using System;
using Microsoft.Practices.Unity;
using NServiceBus;

static class Program
{
    static void Main()
    {
        #region ContainerConfiguration
        var configuration = new BusConfiguration();
        configuration.EndpointName("Samples.Unity");

        var container = new UnityContainer();
        container.RegisterInstance(new MyService());
        configuration.UseContainer<UnityBuilder>(c => c.UseExistingContainer(container));
        #endregion
        configuration.UseSerialization<JsonSerializer>();
        configuration.UsePersistence<InMemoryPersistence>();
        configuration.EnableInstallers();

        using (var bus = Bus.Create(configuration))
        {
            bus.Start();
            bus.SendLocal(new MyMessage());
            Console.WriteLine("Press any key to exit");
            Console.Read();

        }
    }
}
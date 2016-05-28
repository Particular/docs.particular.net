using System;
using Microsoft.Practices.Unity;
using NServiceBus;

static class Program
{
    static void Main()
    {
        Console.Title = "Samples.Unity";
        #region ContainerConfiguration
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Unity");

        var container = new UnityContainer();
        container.RegisterInstance(new MyService());
        busConfiguration.UseContainer<UnityBuilder>(c => c.UseExistingContainer(container));
        #endregion
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            bus.SendLocal(new MyMessage());
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
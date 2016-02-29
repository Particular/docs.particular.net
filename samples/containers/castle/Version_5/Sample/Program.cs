using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NServiceBus;

static class Program
{
    static void Main()
    {
        Console.Title = "Samples.Castle";
        #region ContainerConfiguration
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Castle");

        WindsorContainer container = new WindsorContainer();
        container.Register(Component.For<MyService>().Instance(new MyService()));

        busConfiguration.UseContainer<WindsorBuilder>(c => c.ExistingContainer(container));
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
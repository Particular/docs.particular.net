using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NServiceBus;

static class Program
{
    static void Main()
    {
        #region ContainerConfiguration
        BusConfiguration configuration = new BusConfiguration();
        configuration.EndpointName("Samples.Castle");

        WindsorContainer container = new WindsorContainer();
        container.Register(Component.For<MyService>().Instance(new MyService()));

        configuration.UseContainer<WindsorBuilder>(c => c.ExistingContainer(container));
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
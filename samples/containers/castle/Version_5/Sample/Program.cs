using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NServiceBus;

static class Program
{
    static void Main()
    {
        #region ContainerConfiguration
        var configuration = new BusConfiguration();
        configuration.EndpointName("Samples.Castle");

        var container = new WindsorContainer();
        container.Register(Component.For<MyService>().Instance(new MyService()));

        configuration.UseContainer<WindsorBuilder>(c => c.ExistingContainer(container));
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
using System;
using Autofac;
using NServiceBus;

static class Program
{
    static void Main()
    {
        #region ContainerConfiguration
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Autofac");
        
        ContainerBuilder builder = new ContainerBuilder();
        builder.RegisterInstance(new MyService());
        IContainer container = builder.Build();
        busConfiguration.UseContainer<AutofacBuilder>(c => c.ExistingLifetimeScope(container));
        #endregion
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();

        using (IStartableBus bus = Bus.Create(busConfiguration))
        {
            bus.Start();
            bus.SendLocal(new MyMessage());
            Console.WriteLine("Press any key to exit");
            Console.Read();

        }
    }
}
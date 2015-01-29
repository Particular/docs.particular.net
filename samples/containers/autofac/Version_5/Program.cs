using System;
using Autofac;
using NServiceBus;

static class Program
{
    static void Main()
    {
        #region ContainerConfiguration
        var configuration = new BusConfiguration();
        configuration.EndpointName("Samples.Autofac");
        
        var builder = new ContainerBuilder();
        builder.RegisterInstance(new MyService());
        var container = builder.Build();
        configuration.UseContainer<AutofacBuilder>(c => c.ExistingLifetimeScope(container));
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
using System;
using Autofac;
using NServiceBus;

static class Program
{
    static void Main()
    {
        Console.Title = "Samples.Autofac";

        #region ContainerConfiguration

        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Autofac");

        var builder = new ContainerBuilder();
        builder.RegisterInstance(new MyService());
        var container = builder.Build();
        busConfiguration.UseContainer<AutofacBuilder>(
            customizations: customizations =>
            {
                customizations.ExistingLifetimeScope(container);
            });

        #endregion

        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            var myMessage = new MyMessage();
            bus.SendLocal(myMessage);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
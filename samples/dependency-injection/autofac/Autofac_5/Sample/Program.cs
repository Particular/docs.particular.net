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

        builder
            .Register(x => Bus.Create(busConfiguration).Start())
            .SingleInstance();

        using (var container = builder.Build())
        {
            busConfiguration.UseContainer<AutofacBuilder>(
                customizations: customizations =>
                {
                    // Child container needed as otherwise creation will hang
                    var childContainer = container.BeginLifetimeScope();
                    customizations.ExistingLifetimeScope(childContainer);
                });

            #endregion

            busConfiguration.UsePersistence<InMemoryPersistence>();
            busConfiguration.EnableInstallers();

            var bus = container.Resolve<IBus>();
            var myMessage = new MyMessage();
            bus.SendLocal(myMessage);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
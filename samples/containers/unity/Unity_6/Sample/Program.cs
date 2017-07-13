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
        busConfiguration.UseContainer<UnityBuilder>(
            customizations: customizations =>
            {
                customizations.UseExistingContainer(container);
            });

        #endregion

        busConfiguration.UseSerialization<JsonSerializer>();
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
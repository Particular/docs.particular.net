using System;
using NServiceBus;
using Spring.Context.Support;

static class Program
{
    static void Main()
    {
        Console.Title = "Samples.Spring";

        #region ContainerConfiguration

        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Spring");

        var applicationContext = new GenericApplicationContext();
        applicationContext.ObjectFactory
            .RegisterSingleton("MyService", new MyService());
        busConfiguration.UseContainer<SpringBuilder>(
            customizations: customizations =>
            {
                customizations.ExistingApplicationContext(applicationContext);
            });

        #endregion

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
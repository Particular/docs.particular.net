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
        applicationContext.ObjectFactory.RegisterSingleton("MyService", new MyService());
        busConfiguration.UseContainer<SpringBuilder>(c => c.ExistingApplicationContext(applicationContext));
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
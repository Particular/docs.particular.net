using System;
using NServiceBus;
using Spring.Context.Support;

static class Program
{
    static void Main()
    {
        #region ContainerConfiguration
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Spring");

        GenericApplicationContext applicationContext = new GenericApplicationContext();
        applicationContext.ObjectFactory.RegisterSingleton("MyService", new MyService());
        busConfiguration.UseContainer<SpringBuilder>(c => c.ExistingApplicationContext(applicationContext));
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
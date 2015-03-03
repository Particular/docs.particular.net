using System;
using NServiceBus;
using Spring.Context.Support;

static class Program
{
    static void Main()
    {
        #region ContainerConfiguration
        BusConfiguration configuration = new BusConfiguration();
        configuration.EndpointName("Samples.Spring");

        GenericApplicationContext applicationContext = new GenericApplicationContext();
        applicationContext.ObjectFactory.RegisterSingleton("MyService", new MyService());
        configuration.UseContainer<SpringBuilder>(c => c.ExistingApplicationContext(applicationContext));
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
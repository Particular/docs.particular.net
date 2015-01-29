using System;
using NServiceBus;
using StructureMap;

static class Program
{
    static void Main()
    {
        #region ContainerConfiguration
        var configuration = new BusConfiguration();
        configuration.EndpointName("Samples.StructureMap");

        var container = new Container(x => x.For<MyService>().Use(new MyService()));
        configuration.UseContainer<StructureMapBuilder>(c => c.ExistingContainer(container));
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
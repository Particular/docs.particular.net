using System;
using NServiceBus;
using StructureMap;

static class Program
{
    static void Main()
    {
        Console.Title = "Samples.StructureMap";
        #region ContainerConfiguration
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.StructureMap");

        Container container = new Container(x => x.For<MyService>().Use(new MyService()));
        busConfiguration.UseContainer<StructureMapBuilder>(c => c.ExistingContainer(container));
        #endregion
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            bus.SendLocal(new MyMessage());
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

        }
    }
}
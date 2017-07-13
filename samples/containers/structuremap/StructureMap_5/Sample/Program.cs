using System;
using NServiceBus;
using StructureMap;

static class Program
{
    static void Main()
    {
        Console.Title = "Samples.StructureMap";

        #region ContainerConfiguration

        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.StructureMap");

        var container = new Container(
            action: expression =>
            {
                expression.For<MyService>()
                    .Use(new MyService());
            });
        busConfiguration.UseContainer<StructureMapBuilder>(
            customizations: customizations =>
            {
                customizations.ExistingContainer(container);
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
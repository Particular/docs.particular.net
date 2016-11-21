using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.ComplexSagaFindingLogic";
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.ComplexSagaFindingLogic");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            var startOrder1 = new StartOrder
            {
                OrderId = "1"
            };
            bus.SendLocal(startOrder1);

            var startOrder2 = new StartOrder
            {
                OrderId = "2"
            };
            bus.SendLocal(startOrder2);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}

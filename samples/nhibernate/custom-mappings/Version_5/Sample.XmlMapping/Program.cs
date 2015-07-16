using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.CustomMappings");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<NHibernatePersistence>();

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            bus.SendLocal(new StartOrder
            {
                OrderId = "123"
            });

            bus.SendLocal(new StartOrder
            {
                OrderId = "123"
            });

            bus.SendLocal(new StartOrder
            {
                OrderId = "123"
            });

            bus.SendLocal(new StartOrder
            {
                OrderId = "456"
            });

            bus.SendLocal(new CompleteOrder
            {
                OrderId = "123"
            });

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}

using System;
using System.Collections.Generic;
using NServiceBus;
using NServiceBus.Wire;

static class Program
{
    static void Main()
    {
        Console.Title = "Samples.Serialization.Wire";
        #region config
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Serialization.Wire");
        busConfiguration.UseSerialization<WireSerializer>();
        #endregion
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            #region message
            CreateOrder message = new CreateOrder
            {
                OrderId = 9,
                Date = DateTime.Now,
                CustomerId = 12,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        ItemId = 6,
                        Quantity = 2
                    },
                    new OrderItem
                    {
                        ItemId = 5,
                        Quantity = 4
                    },
                }
            };
            bus.SendLocal(message);
            #endregion
            Console.WriteLine("Order Sent");
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
using System;
using System.Collections.Generic;
using Jil;
using NServiceBus;
using NServiceBus.Jil;

static class Program
{
    static void Main()
    {
        Console.Title = "Samples.Serialization.Jil";
        #region config
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Serialization.Jil");
        busConfiguration.UseSerialization<JilSerializer>()
            .Options(
                new Options(
                    prettyPrint: true,
                    excludeNulls: true,
                    includeInherited: true));
        // register the mutator so the the message on the wire is written
        busConfiguration.RegisterComponents(components =>
        {
            components.ConfigureComponent<MessageBodyWriter>(DependencyLifecycle.InstancePerCall);
        });
        #endregion
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            #region message
            var message = new CreateOrder
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
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NServiceBus;
using NServiceBus.Newtonsoft.Json;

static class Program
{
    static void Main()
    {
        Console.Title = "Samples.Serialization.ExternalJson";
        #region config
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Serialization.ExternalJson"); 
        var settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented
        };
        busConfiguration.UseSerialization<NewtonsoftSerializer>()
            .Settings(settings);
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
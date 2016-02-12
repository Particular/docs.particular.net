using System;
using System.Collections.Generic;
using Newtonsoft.Json.Bson;
using NServiceBus;
using NServiceBus.Newtonsoft.Json;
using NServiceBus.Serialization;

static class Program
{
    static void Main()
    {
        #region config
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Serialization.ExternalBson");
        SerializationExtentions<NewtonsoftSerializer> serialization =
            busConfiguration.UseSerialization<NewtonsoftSerializer>();
        serialization.ReaderCreator(stream => new BsonReader(stream));
        serialization.WriterCreator(stream => new BsonWriter(stream));
        // register the mutator so the the message on the wire is written
        busConfiguration.RegisterComponents(components =>
        {
            components.ConfigureComponent<MessageBodyWriter>(DependencyLifecycle.InstancePerCall);
        });
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
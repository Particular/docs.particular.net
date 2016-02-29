using System;
using System.Collections.Generic;
using NServiceBus;
using XmlSample;

static class Program
{
    static void Main()
    {
        Console.Title = "Samples.Serialization.Xml";
        #region config
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Serialization.Xml");
        // this is optional since Xml is the default serializer
        busConfiguration.UseSerialization<XmlSerializer>();
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
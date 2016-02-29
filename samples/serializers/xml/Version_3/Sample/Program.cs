using System;
using System.Collections.Generic;
using NServiceBus;
using NServiceBus.Installation.Environments;
using XmlSample;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.Serialization.Xml";
        #region config
        Configure configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.Serialization.Xml");
        configure.DefaultBuilder();
        // this is optional since Xml is the default serializer
        configure.XmlSerializer();
        // register the mutator so the the message on the wire is written
        configure.Configurer
            .ConfigureComponent<MessageBodyWriter>(DependencyLifecycle.InstancePerCall);
        #endregion
        configure.MsmqTransport();
        configure.InMemorySagaPersister();
        configure.RunTimeoutManagerWithInMemoryPersistence();
        configure.InMemorySubscriptionStorage();
        using (IStartableBus startableBus = configure.UnicastBus().CreateBus())
        {
            IBus bus = startableBus.Start(() => configure.ForInstallationOn<Windows>().Install());
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
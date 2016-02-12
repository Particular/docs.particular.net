using System;
using System.Collections.Generic;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        #region config
        Configure.Serialization.Bson();
        Configure configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.Serialization.Bson");
        configure.DefaultBuilder();
        // register the mutator so the the message on the wire is written
        configure.Configurer
            .ConfigureComponent<MessageBodyWriter>(DependencyLifecycle.InstancePerCall);
        #endregion
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.UseTransport<Msmq>();
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
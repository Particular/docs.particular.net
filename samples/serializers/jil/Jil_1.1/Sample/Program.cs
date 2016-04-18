using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jil;
using NServiceBus;
using NServiceBus.Jil;

static class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.Serialization.Jil";

        #region config

        EndpointConfiguration endpointConfiguration = new EndpointConfiguration("Samples.Serialization.Jil");
        endpointConfiguration.UseSerialization<JilSerializer>()
            .Options(
                new Options(
                    prettyPrint: true,
                    excludeNulls: true,
                    includeInherited: true));
        // register the mutator so the the message on the wire is written
        endpointConfiguration.RegisterComponents(components =>
        {
            components.ConfigureComponent<MessageBodyWriter>(DependencyLifecycle.InstancePerCall);
        });

        #endregion

        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
        try
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
            await endpoint.SendLocal(message);

            #endregion

            Console.WriteLine("Order Sent");
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        finally
        {
            await endpoint.Stop();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NServiceBus;

static class Program
{
    static void Main()
    {
        #region config
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Serialization.ExternalJson"); 
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented
        };
        busConfiguration.UseSerialization<NewtonsoftSerialization>()
            .Settings(settings);
        // register the mutator so the the message on the wire is written
        busConfiguration.RegisterComponents(components =>
        {
            components.ConfigureComponent<MessageBodyWriter>(DependencyLifecycle.InstancePerCall);
        });
        #endregion
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();

        Run(busConfiguration).GetAwaiter().GetResult();
    }

    static async Task Run(BusConfiguration busConfiguration)
    {
        IEndpointInstance bus = await Endpoint.Start(busConfiguration);
        IBusSession session = bus.CreateBusSession();
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
        await session.SendLocal(message);

        #endregion

        Console.WriteLine("Order Sent");
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
    }
}
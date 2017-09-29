using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jil;
using NServiceBus;
using NServiceBus.Jil;
using NServiceBus.MessageMutator;

static class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Serialization.Jil";

        #region config

        var endpointConfiguration = new EndpointConfiguration("Samples.Serialization.Jil");
        endpointConfiguration.UseSerialization<JilSerializer>()
            .Options(
                new Options(
                    prettyPrint: true,
                    excludeNulls: true,
                    includeInherited: true));
        // register the mutator so the the message on the wire is written
        endpointConfiguration.RegisterMessageMutator(new MessageBodyWriter());

        #endregion

        endpointConfiguration.UseTransport<LearningTransport>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
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
        await endpointInstance.SendLocal(message)
            .ConfigureAwait(false);

        #endregion

        Console.WriteLine("Order Sent");
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}
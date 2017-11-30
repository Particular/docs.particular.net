using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Bson;
using NServiceBus;
using NServiceBus.MessageMutator;

static class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Serialization.ExternalBson";

        #region config

        var endpointConfiguration = new EndpointConfiguration("Samples.Serialization.ExternalBson");
        var serialization = endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
        serialization.ReaderCreator(stream => new BsonDataReader(stream));
        serialization.WriterCreator(stream => new BsonDataWriter(stream));

        #endregion

        #region registermutator

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
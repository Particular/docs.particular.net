﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.MessageMutator;
using XmlSample;

static class Program
{
    static async Task Main()
    {
        Console.Title = "Xml";
        #region config
        var endpointConfiguration = new EndpointConfiguration("Samples.Serialization.Xml");
        // this is optional since Xml is the default serializer
        endpointConfiguration.UseSerialization<XmlSerializer>();
        // register the mutator so the the message on the wire is written
        endpointConfiguration.RegisterMessageMutator(new MessageBodyWriter());
        #endregion
        endpointConfiguration.UseTransport<LearningTransport>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
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
        await endpointInstance.SendLocal(message);
        #endregion
        Console.WriteLine("Order Sent");
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}
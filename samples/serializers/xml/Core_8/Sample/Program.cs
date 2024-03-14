using System;
using System.Collections.Generic;
using NServiceBus;
using NServiceBus.MessageMutator;


Console.Title = "Samples.Serialization.Xml";

#region config
var endpointConfiguration = new EndpointConfiguration("Samples.Serialization.Xml");

endpointConfiguration.UseSerialization<XmlSerializer>();

// register the mutator so the the message on the wire is written
endpointConfiguration.RegisterMessageMutator(new MessageBodyWriter());
#endregion

endpointConfiguration.UseTransport(new LearningTransport());

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

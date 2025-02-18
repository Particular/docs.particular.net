using System;
using NServiceBus;

Console.Title = "Client";
var endpointConfiguration = new EndpointConfiguration("Samples.NHibernate.Client");
endpointConfiguration.EnableInstallers();
endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

var endpointInstance = await Endpoint.Start(endpointConfiguration);

Console.WriteLine("Press 'enter' to send a StartOrder messages");
Console.WriteLine("Press any other key to exit");

while (true)
{
    var key = Console.ReadKey();
    Console.WriteLine();

    if (key.Key != ConsoleKey.Enter)
    {
        break;
    }

    var orderId = Guid.NewGuid();
    var startOrder = new StartOrder
    {
        OrderId = orderId
    };
    await endpointInstance.Send("Samples.NHibernate.Server", startOrder);
    Console.WriteLine($"StartOrder Message sent with OrderId {orderId}");
}

await endpointInstance.Stop();
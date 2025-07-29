using System;
using System.Threading.Tasks;
using NServiceBus;

Console.Title = "Client";

var endpointConfiguration = new EndpointConfiguration("Samples.FaultTolerance.Client");
endpointConfiguration.UsePersistence<LearningPersistence>();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

var endpointInstance = await Endpoint.Start(endpointConfiguration);

Console.WriteLine("Press enter to send a message");
Console.WriteLine("Press any key to exit");

while (true)
{
    var key = Console.ReadKey();
    if (key.Key != ConsoleKey.Enter)
    {
        break;
    }

    var id = Guid.NewGuid();
    var myMessage = new MyMessage
    {
        Id = id
    };

    await endpointInstance.Send("Samples.FaultTolerance.Server", myMessage);
    Console.WriteLine($"Sent a message with id: {id:N}");
}

await endpointInstance.Stop();

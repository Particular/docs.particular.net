using System;
using System.Threading.Tasks;
using NServiceBus;

Console.Title = "Endpoint1";

#region config

var endpointConfiguration = new EndpointConfiguration("Samples.ASBS.SendReply.Endpoint1");
endpointConfiguration.EnableInstallers();

var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus_ConnectionString");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new Exception("Could not read the 'AzureServiceBus_ConnectionString' environment variable. Check the sample prerequisites.");
}

var transport = new AzureServiceBusTransport(connectionString);
endpointConfiguration.UseTransport(transport);
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

#endregion

var endpointInstance = await Endpoint.Start(endpointConfiguration);
Console.WriteLine("Press 'enter' to send a message");
Console.WriteLine("Press any other key to exit");

while (true)
{
    var key = Console.ReadKey();
    Console.WriteLine();

    if (key.Key != ConsoleKey.Enter)
    {
        break;
    }

    var message = new Message1
    {
        Property = "Hello from Endpoint1"
    };
    await endpointInstance.Send("Samples.ASBS.SendReply.Endpoint2", message);
    Console.WriteLine("Message1 sent");
}
await endpointInstance.Stop();
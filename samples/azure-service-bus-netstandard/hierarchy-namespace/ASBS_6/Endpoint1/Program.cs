using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Transport.AzureServiceBus;

Console.Title = "Endpoint1";

#region config

var endpointConfiguration = new EndpointConfiguration("Samples.ASBS.SendReply.Endpoint1");
endpointConfiguration.EnableInstallers();

var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus_ConnectionString");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new Exception("Could not read the 'AzureServiceBus_ConnectionString' environment variable. Check the sample prerequisites.");
}

var transport = new AzureServiceBusTransport(connectionString, TopicTopology.Default);
transport.HierarchyNamespaceOptions = new HierarchyNamespaceOptions { HierarchyNamespace = "my-hierarchy" };
transport.HierarchyNamespaceOptions.ExcludeMessageType<MessageExcluded>();
endpointConfiguration.UseTransport(transport);
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

#endregion

Console.WriteLine("Starting...");

var builder = Host.CreateApplicationBuilder(args);
builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();

await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

Console.WriteLine("Press [1] to send a message to endpoint2 to be replied to");
Console.WriteLine("Press [2] to send a message excluded from the hierarchy to endpoint2 & endpoint3");
Console.WriteLine("Press any other key to exit");

while (true)
{
    var key = Console.ReadKey();
    Console.WriteLine();

    if (key.Key == ConsoleKey.D1)
    {
        var message = new Message1
        {
            Property = "Hello from Endpoint1"
        };
        await messageSession.Send("Samples.ASBS.SendReply.Endpoint2", message);
        Console.WriteLine("Message1 sent");
        continue;
    }

    if (key.Key == ConsoleKey.D2)
    {
        var message = new MessageExcluded
        {
            Property = "Hello from Endpoint1 - excluded from hierarchy"
        };
        await messageSession.Send("Samples.ASBS.SendReply.Endpoint2", message);
        await messageSession.Send("Samples.ASBS.SendReply.Endpoint3", message); 
        Console.WriteLine("MessageExcluded sent");
        continue;
    }

    break;
}

await host.StopAsync();

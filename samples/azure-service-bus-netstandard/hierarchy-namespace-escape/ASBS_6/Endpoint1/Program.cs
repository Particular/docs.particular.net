using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Transport.AzureServiceBus;

Console.Title = "Endpoint1";

#region config

var endpointConfiguration = new EndpointConfiguration("Samples.ASBS.HierarchyNamespaceEscape.Endpoint1");
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

Console.WriteLine("Press [2] to send an excluded message to endpoint2 (my-hierarchy/..endpoint2)");
Console.WriteLine("Press [3] to send an excluded message to endpoint3 (..endpoint3)");
Console.WriteLine("Press any other key to exit");

while (true)
{
    var key = Console.ReadKey();
    Console.WriteLine();

    if (key.Key == ConsoleKey.D2)
    {
        var message = new MessageExcluded()
        {
            Property = "Hello from Endpoint1 - excluded from hierarchy"
        };
        try
        {
            await messageSession.Send("Samples.ASBS.HierarchyNamespaceEscape.Endpoint2", message);
            Console.WriteLine("MessageExcluded to endpoint 2 sent");
        }
        catch (Exception)
        {
            Console.WriteLine("Sending a MessageExcluded to endpoint 2 failed");
        }

        continue;
    }

    if (key.Key == ConsoleKey.D3)
    {
        var message = new MessageExcluded
        {
            Property = "Hello from Endpoint1 - excluded from hierarchy"
        };
        await messageSession.Send("Samples.ASBS.HierarchyNamespaceEscape.Endpoint3", message);
        Console.WriteLine("MessageExcluded to endpoint 3 sent");
        continue;
    }

    break;
}

await host.StopAsync();

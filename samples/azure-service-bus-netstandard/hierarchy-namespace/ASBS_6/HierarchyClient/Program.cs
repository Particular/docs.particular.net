using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Transport.AzureServiceBus;
using Shared;

Console.Title = "HierarchyClient";

#region config

var endpointConfiguration = new EndpointConfiguration("Samples.ASBS.HierarchyNamespace.HierarchyClient");
endpointConfiguration.EnableInstallers();

var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus_ConnectionString");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new Exception("Could not read the 'AzureServiceBus_ConnectionString' environment variable. Check the sample prerequisites.");
}

var transport = new AzureServiceBusTransport(connectionString, TopicTopology.Default);
#region namespaceOptions
transport.HierarchyNamespaceOptions = new HierarchyNamespaceOptions { HierarchyNamespace = "my-hierarchy" };
#endregion

#region excludedMessage
// Exclude a single concrete type
transport.HierarchyNamespaceOptions.ExcludeMessageType<ExternalCommand>();
// Exclude all types by inheritance
transport.HierarchyNamespaceOptions.ExcludeMessageType<IExternalEvent>();
#endregion
endpointConfiguration.UseTransport(transport);
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

#endregion

Console.WriteLine("Starting...");

var builder = Host.CreateApplicationBuilder(args);
builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();

await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

while (true)
{
    Console.WriteLine("Press [1] to send a hierarchy command to hierarchy and external endpoints");
    Console.WriteLine("Press [2] to send an external command to hierarchy and external receivers");
    Console.WriteLine("Press [3] to publish a hierarchy event");
    Console.WriteLine("Press [4] to publish an external event");
    Console.WriteLine("Press any other key to exit");
    var key = Console.ReadKey();


    if (key.Key == ConsoleKey.D1)
    {
        Console.WriteLine("Sending a hierarchy commands (should not be received by ExternalEndpoint)...");

        var hierarchyCommandToHierarchy = new HierarchyCommand { Source = "HierarchyClient", Destination = "HierarchyEndpoint" };
        var hierarchyCommandToExternal = new HierarchyCommand { Source = "HierarchyClient", Destination = "ExternalEndpoint" };

        await messageSession.Send("Samples.ASBS.HierarchyNamespace.HierarchyEndpoint", hierarchyCommandToHierarchy);
        await messageSession.Send("Samples.ASBS.HierarchyNamespace.ExternalEndpoint", hierarchyCommandToExternal);

        Console.WriteLine("Sending successful!");
        continue;
    }

    if (key.Key == ConsoleKey.D2)
    {
        Console.WriteLine("Sending external commands (should not be received by HierarchyEndpoint)...");

        var externalCommandToExternal = new ExternalCommand { Source = "HierarchyClient", Destination = "ExternalEndpoint" };
        var externalCommandToHierarchy = new ExternalCommand { Source = "HierarchyClient", Destination = "HierarchyEndpoint" };

        await messageSession.Send("Samples.ASBS.HierarchyNamespace.ExternalEndpoint", externalCommandToExternal);
        await messageSession.Send("Samples.ASBS.HierarchyNamespace.HierarchyEndpoint", externalCommandToHierarchy);

        Console.WriteLine("Sending successful!");
        continue;
    }

    if (key.Key == ConsoleKey.D3)
    {
        Console.WriteLine("Publishing a hierarchy event (should not be received by ExternalEndpoint)...");

        var hierarchyEvent = new HierarchyEvent { Source = "HierarchyClient" };

        await messageSession.Publish(hierarchyEvent);

        Console.WriteLine("Event published!");
        continue;
    }

    if (key.Key == ConsoleKey.D4)
    {
        Console.WriteLine("Publishing excluded events (should not be received by HierarchyEndpoint)...");

        var hierarchyEvent = new ExternalEvent { Source = "HierarchyClient" };
        var otherHierarchyEvent = new OtherExternalEvent { Source = "HierarchyClient" };

        await messageSession.Publish(hierarchyEvent);
        await messageSession.Publish(otherHierarchyEvent);

        Console.WriteLine("Events published!");
        continue;
    }

    break;
}

await host.StopAsync();

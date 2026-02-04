using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Transport.AzureServiceBus;

Console.Title = "Endpoint1";

var endpointConfiguration = new EndpointConfiguration("Samples.ASBS.HierarchyNamespace.Endpoint1");
endpointConfiguration.EnableInstallers();

var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus_ConnectionString");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new Exception("Could not read the 'AzureServiceBus_ConnectionString' environment variable. Check the sample prerequisites.");
}

var transport = new AzureServiceBusTransport(connectionString, TopicTopology.Default);
#region config
transport.HierarchyNamespaceOptions = new HierarchyNamespaceOptions { HierarchyNamespace = "my-hierarchy" };
#endregion
endpointConfiguration.UseTransport(transport);
endpointConfiguration.UseSerialization<SystemJsonSerializer>();


Console.WriteLine("Starting...");

var builder = Host.CreateApplicationBuilder(args);
builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();

await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

Console.WriteLine("Press [Enter] to send a message to endpoint2 to be replied to");
Console.WriteLine("Press any other key to exit");

while (true)
{
    var key = Console.ReadKey();
    Console.WriteLine();

    if (key.Key == ConsoleKey.Enter)
    {
        var message = new Message1
        {
            Property = "Hello from Endpoint1"
        };
        await messageSession.Send("Samples.ASBS.HierarchyNamespace.Endpoint2", message);
        Console.WriteLine("Message1 sent");
        continue;
    }

    break;
}

await host.StopAsync();

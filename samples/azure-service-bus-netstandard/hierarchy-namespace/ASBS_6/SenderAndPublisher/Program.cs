using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Transport.AzureServiceBus;
using Shared;

Console.Title = "SenderAndPublisher";

#region config

var endpointConfiguration = new EndpointConfiguration("Samples.ASBS.HierarchyNamespace.SenderAndPublisher");
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
transport.HierarchyNamespaceOptions.ExcludeMessageType<ExcludedMessage>();
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
    Console.WriteLine("Press [1] to send a regular message to both receivers");
    Console.WriteLine("Press [2] to send an excluded message to both receivers");
    Console.WriteLine("Press [3] to publish an event");
    Console.WriteLine("Press any other key to exit");
    var key = Console.ReadKey();


    if (key.Key == ConsoleKey.D1)
    {
        Console.WriteLine("Sending a regular message to both receivers...");
        var messageForReceiver1 = new RegularMessage
        {
            Property = "Regular message - to Receiver1"
        };
        var messageForReceiver2 = new RegularMessage
        {
            Property = "Regular message - to Receiver2"
        };

        try
        {
            Console.WriteLine("Sending a regular message to Receiver1...");
            await messageSession.Send("Samples.ASBS.HierarchyNamespace.Receiver1", messageForReceiver1);
            Console.WriteLine("Sending successful!");
        }
        catch (Exception)
        {
            Console.WriteLine("Sending failed.");
        }

        try
        {
            Console.WriteLine("Sending a regular message to Receiver2...");
            await messageSession.Send("Samples.ASBS.HierarchyNamespace.Receiver2", messageForReceiver1);
            Console.WriteLine("Sending successful!");
        }
        catch (Exception)
        {
            Console.WriteLine("Sending failed.");
        }

        continue;
    }

    if (key.Key == ConsoleKey.D2)
    {
        Console.WriteLine("Sending an excluded message to both receivers...");

        var messageForReceiver1 = new ExcludedMessage
        {
            Property = "Excluded message - to Receiver1"
        };
        var messageForReceiver2 = new ExcludedMessage
        {
            Property = "Excluded message - to Receiver2"
        };

        try
        {
            Console.WriteLine("Sending an excluded message to Receiver1...");
            await messageSession.Send("Samples.ASBS.HierarchyNamespace.Receiver1", messageForReceiver1);
            Console.WriteLine("Sending successful!");
        }
        catch (Exception)
        {
            Console.WriteLine("Sending failed.");
        }

        try
        {
            Console.WriteLine("Sending an excluded message to Receiver2...");
            await messageSession.Send("Samples.ASBS.HierarchyNamespace.Receiver2", messageForReceiver1);
            Console.WriteLine("Sending successful!");
        }
        catch (Exception)
        {
            Console.WriteLine("Sending failed.");
        }

        continue;
    }

    if (key.Key == ConsoleKey.D3)
    {
        Console.WriteLine("Publishing an event...");


        var eventToSend = new SampleEvent
        {
            Property = "Hello from SenderAndPublisher - event"
        };
        await messageSession.Publish(eventToSend);
        Console.WriteLine("Event published!");
        continue;
    }

    break;
}

await host.StopAsync();

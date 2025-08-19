using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Shared;

var host = Host.CreateDefaultBuilder(args)
    .UseNServiceBus(x =>
    {
        #region endpointName

        var endpointName = "Samples.Azure.StorageQueues.Endpoint1.With.A.Very.Long.Name.And.Invalid.Characters";
        var endpointConfiguration = new EndpointConfiguration(endpointName);

        #endregion

        Console.Title = endpointName;

        #region config

        var transport = new AzureStorageQueueTransport("UseDevelopmentStorage=true");
        var routingSettings = endpointConfiguration.UseTransport(transport);
        routingSettings.RouteToEndpoint(typeof(MyRequest), "Samples-Azure-StorageQueues-Endpoint2");

        #endregion

        #region sanitization

        transport.QueueNameSanitizer = BackwardsCompatibleQueueNameSanitizer.WithMd5Shortener;

        #endregion

        routingSettings.DisablePublishing();
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.EnableInstallers();

        return endpointConfiguration;
    }).Build();

await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

Console.WriteLine("Press 'enter' to send a message");
while (true)
{
    var key = Console.ReadKey();
    Console.WriteLine();

    if (key.Key != ConsoleKey.Enter)
    {
        break;
    }

    var message = new MyRequest("Hello from Endpoint1");

    await messageSession.Send(message);

    Console.WriteLine("MyRequest sent");
}

await host.StopAsync();
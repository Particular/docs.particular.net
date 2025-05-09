using System;
using Endpoint1;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) => { services.AddHostedService<InputLoopService>(); })
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
        routingSettings.RouteToEndpoint(typeof(Endpoint2.MyRequest), "Samples-Azure-StorageQueues-Endpoint2");

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

await host.RunAsync();
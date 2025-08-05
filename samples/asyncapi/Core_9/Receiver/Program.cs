﻿using Microsoft.Extensions.Hosting;
using NServiceBus;

await Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        //services.AddHostedService<Worker>();
    })
    .UseNServiceBus(builder =>
    {
        var endpointConfiguration = new EndpointConfiguration("Receiver");
        endpointConfiguration.UseTransport<LearningTransport>();
        return endpointConfiguration;
    })
    .Build()
    .RunAsync();
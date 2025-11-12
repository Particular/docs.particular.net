using System;
using NServiceBus;
using Microsoft.Extensions.Hosting;

Console.Title = "Server";

var builder = Host.CreateDefaultBuilder(args);

builder.UseNServiceBus(_ =>
{
    var endpointConfiguration = new EndpointConfiguration("Samples.Web.Server");
    endpointConfiguration.UseTransport(new LearningTransport());

    return endpointConfiguration;
});

await builder.Build().RunAsync();
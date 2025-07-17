using System;
using Microsoft.Extensions.Hosting;
using NServiceBus;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
{
})
.UseNServiceBus(context =>
    {
        var endpointConfiguration = new EndpointConfiguration("Samples.Blazor.Server");
        endpointConfiguration.EnableCallbacks(makesRequests: false);
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport(new LearningTransport());
        return endpointConfiguration;
    }).Build();


await host.StartAsync();
Console.WriteLine("Press any key to exit");
Console.ReadKey();

await host.StopAsync();


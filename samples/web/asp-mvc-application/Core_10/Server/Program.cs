using System;
using Microsoft.Extensions.Hosting;
using NServiceBus;



var host = Host.CreateDefaultBuilder(args)
.ConfigureServices((hostContext, services) =>
{
})
.UseNServiceBus(context =>
{
    Console.Title = "MvcServer";
    var endpointConfiguration = new EndpointConfiguration("Samples.Mvc.Server");
    endpointConfiguration.EnableCallbacks(makesRequests: false);
    endpointConfiguration.UseTransport(new LearningTransport());
    endpointConfiguration.UseSerialization<SystemJsonSerializer>();

    return endpointConfiguration;
}).Build();


await host.StartAsync();
Console.WriteLine("Press any key to exit");
Console.ReadKey();

await host.StopAsync();
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "Publisher";

var endpointConfiguration = new EndpointConfiguration("Samples.ASB.Publisher");

endpointConfiguration.EnableInstallers();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.Conventions().DefiningEventsAs(type => type.Name == nameof(EventTwo) || type.Name == nameof(EventOne));


var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus_ConnectionString");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new Exception("Could not read the 'AzureServiceBus_ConnectionString' environment variable. Check the sample prerequisites.");
}

var transport = new AzureServiceBusTransport(connectionString, TopicTopology.Default);
endpointConfiguration.UseTransport(transport);

var builder = Host.CreateApplicationBuilder();
builder.Services.AddNServiceBusEndpoint(endpointConfiguration);
var host = builder.Build();
var messageSession = host.Services.GetRequiredService<IMessageSession>();
await host.StartAsync();

Console.WriteLine("Press any key to publish events");
Console.ReadKey();
Console.WriteLine();

await messageSession.Publish(new EventOne
{
    Content = $"{nameof(EventOne)} sample content",
    PublishedOnUtc = DateTime.UtcNow
});

await messageSession.Publish(new EventTwo
{
    Content = $"{nameof(EventTwo)} sample content",
    PublishedOnUtc = DateTime.UtcNow
});

Console.WriteLine("Press any key to exit");
Console.ReadKey();
await host.StopAsync();
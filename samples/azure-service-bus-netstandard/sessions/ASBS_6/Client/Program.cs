using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared;

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddConsole();

var endpointConfiguration = new EndpointConfiguration("Samples.OrderedDelivery.Client");

var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus_ConnectionString")
                       ?? throw new Exception("Please set up connection string using environment variable 'AzureServiceBus_ConnectionString'");
var routing = endpointConfiguration.UseTransport(new AzureServiceBusTransport(connectionString, TopicTopology.Default));
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.SendOnly();

routing.RouteToEndpoint(typeof(AddItem), "Samples.OrderedDelivery.Sales");
routing.RouteToEndpoint(typeof(PlaceOrder), "Samples.OrderedDelivery.Sales");

// Operational scripting: https://docs.particular.net/transports/azure-service-bus/operational-scripting
endpointConfiguration.EnableInstallers();

builder.Services.AddNServiceBusEndpoint(endpointConfiguration);

using var host = builder.Build();

await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

var orderId = Guid.NewGuid();
var sendOptions = new SendOptions();
// Set the sessionid to a value that correlates to all the messages you want to have ordered
sendOptions.SetSessionId(orderId.ToString());

await messageSession.Send(new AddItem { OrderId = orderId, Name = "Coffee" }, sendOptions);
Console.WriteLine($"Added coffee to order {orderId}");

await messageSession.Send(new AddItem { OrderId = orderId, Name = "Sugar" }, sendOptions);
Console.WriteLine($"Added sugar to order {orderId}");

await messageSession.Send(new AddItem { OrderId = orderId, Name = "Doughnuts" }, sendOptions);
Console.WriteLine($"Added doughnuts to order {orderId}");

await messageSession.Send(new PlaceOrder { OrderId = orderId }, sendOptions);
Console.WriteLine($"Issuing PlaceOrder command for order {orderId}");


await host.StopAsync();
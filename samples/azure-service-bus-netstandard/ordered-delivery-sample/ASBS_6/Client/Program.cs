using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared;

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddConsole();

var endpointConfiguration = new EndpointConfiguration("Client");

var connectionString = builder.Configuration.GetConnectionString("AzureServiceBusConnectionString") ?? string.Empty;
var routing = endpointConfiguration.UseTransport(new AzureServiceBusTransport(connectionString, TopicTopology.Default));
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.SendOnly();

routing.RouteToEndpoint(typeof(AddItem), "Sales");
routing.RouteToEndpoint(typeof(PlaceOrder), "Sales");

// Operational scripting: https://docs.particular.net/transports/azure-service-bus/operational-scripting
endpointConfiguration.EnableInstallers();

builder.Services.AddNServiceBusEndpoint(endpointConfiguration);

using var host = builder.Build();

await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

var orderId = Guid.NewGuid();
var sendOptions = new SendOptions();
sendOptions.SetSessionId(orderId.ToString());

await messageSession.Send(new AddItem { OrderId = orderId, Name = "Coffee"}, sendOptions);
Console.WriteLine("Added coffee to order");

await messageSession.Send(new AddItem{ OrderId = orderId, Name = "Sugar"}, sendOptions);
Console.WriteLine("Added sugar to order");

await messageSession.Send(new AddItem{ OrderId = orderId, Name = "Doughnuts"}, sendOptions);
Console.WriteLine("Added doughnuts to order");

await messageSession.Send(new PlaceOrder{ OrderId = orderId}, sendOptions);
Console.WriteLine("Issuing PlaceOrder command");

await host.StopAsync();
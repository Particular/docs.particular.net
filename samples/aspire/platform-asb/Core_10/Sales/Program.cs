using Microsoft.Extensions.Hosting;

Console.Title = "Sales";

var builder = Host.CreateApplicationBuilder();

builder
    .AddServiceDefaults()
    .AddNServiceBusEndpoint("Sales");

await builder.Build().RunAsync();
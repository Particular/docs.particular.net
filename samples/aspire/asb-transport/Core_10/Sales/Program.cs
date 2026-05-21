
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder();

builder
    .AddServiceDefaults()
    .AddNServiceBusEndpoint("Sales");

await builder.Build().RunAsync();

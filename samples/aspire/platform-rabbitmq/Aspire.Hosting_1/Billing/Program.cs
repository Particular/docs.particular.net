using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder();

builder
    .AddServiceDefaults()
    .AddNServiceBusEndpoint("Billing");

await builder.Build().RunAsync();

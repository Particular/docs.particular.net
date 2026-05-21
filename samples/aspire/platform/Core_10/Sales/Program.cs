using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder();

builder.AddServiceDefaults();
builder.AddNServiceBusEndpoint("Sales");

await builder.Build().RunAsync();


using Microsoft.Extensions.Hosting;

#region endpoint-config
var builder = Host.CreateApplicationBuilder();

builder
    .AddServiceDefaults()
    .AddNServiceBusEndpoint("Sales");

await builder.Build().RunAsync();
#endregion
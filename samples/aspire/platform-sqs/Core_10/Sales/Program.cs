using Microsoft.Extensions.Hosting;

Console.Title = "Sales";

#region endpoint-config
var builder = Host.CreateApplicationBuilder(args);

builder
    .AddServiceDefaults()
    .AddNServiceBusEndpoint("Sales");

await builder.Build().RunAsync();
#endregion
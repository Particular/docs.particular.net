using Microsoft.Extensions.Hosting;

Console.Title = "Sales";

var builder = Host.CreateApplicationBuilder(args);

builder.AddNServiceBusEndpoint("Sales");

await builder.Build().RunAsync();
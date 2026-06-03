using AzureFunctions.ServiceBus;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;

#region service-bus-program-builder

var builder = FunctionsApplication.CreateBuilder(args);

builder.AddNServiceBusFunctions();

var host = builder.Build();
await host.RunAsync();

#endregion

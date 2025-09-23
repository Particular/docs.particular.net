using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;

#region configuration-with-function-host-builder
[assembly: NServiceBusTriggerFunction("ASBWorkerEndpoint")]

var builder = FunctionsApplication.CreateBuilder(args);

builder.AddNServiceBus();

var host = builder.Build();

await host.RunAsync();
#endregion
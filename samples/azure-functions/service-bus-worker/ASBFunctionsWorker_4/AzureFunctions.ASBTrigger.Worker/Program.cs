using Microsoft.Extensions.Hosting;
using NServiceBus;

#region configuration-with-function-host-builder
[assembly: NServiceBusTriggerFunction("ASBWorkerEndpoint")]

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .UseNServiceBus()
    .Build();

await host.RunAsync();
#endregion
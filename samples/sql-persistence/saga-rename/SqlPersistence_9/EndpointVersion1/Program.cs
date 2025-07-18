using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Shared;

Console.Title = "EndpointVersion1";

// Cnfigure the endpoint
var endpointConfiguration = new EndpointConfiguration("Samples.RenameSaga");
SharedConfiguration.Apply(endpointConfiguration);
endpointConfiguration.PurgeOnStartup(true);

var builder = Host.CreateApplicationBuilder(args);
builder.UseNServiceBus(endpointConfiguration);

// Build and start the host
var host = builder.Build();
await host.StartAsync();

// Get required services
var logger = host.Services.GetRequiredService<ILogger<Program>>();
var messageSession = host.Services.GetRequiredService<IMessageSession>();

logger.LogInformation("EndpointVersion1 of Sagas starting. Will exit in 5 seconds. After exit, start Phase 2 Endpoint.");

#region startSagas
var startReplySaga = new StartReplySaga
{
    TheId = Guid.NewGuid()
};

await messageSession.SendLocal(startReplySaga);

var startTimeoutSaga = new StartTimeoutSaga
{
    TheId = Guid.NewGuid()
};

await messageSession.SendLocal(startTimeoutSaga);
#endregion

await Task.Delay(TimeSpan.FromSeconds(5));

await host.StopAsync();

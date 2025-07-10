using System;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.Cooperative.Cancellation");
endpointConfiguration.UsePersistence<LearningPersistence>();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();
await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

await messageSession.SendLocal(new LongRunningMessage { DataId = Guid.NewGuid() }, CancellationToken.None);

Console.ReadKey();

var logger = host.Services.GetService<ILogger<Program>>();
logger.LogInformation("Giving the endpoint 1 second to gracefully stop before sending a cancel signal to the cancellation token");

#region StoppingEndpointWithCancellationToken
var tokenSource = new CancellationTokenSource();
tokenSource.CancelAfter(TimeSpan.FromSeconds(1));
await host.StopAsync(tokenSource.Token);
#endregion



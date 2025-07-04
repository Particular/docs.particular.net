using System;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Server;


var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.Cooperative.Cancellation");
endpointConfiguration.UsePersistence<LearningPersistence>();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

builder.UseNServiceBus(endpointConfiguration);
builder.Services.AddHostedService<InputLoopService>();

var host =  builder.Build();
await host.StartAsync();

Console.ReadKey();

ILogger logger = host.Services.GetService<ILogger<Program>>();
logger.LogInformation("Giving the endpoint 1 second to gracefully stop before sending a cancel signal to the cancellation token");

#region StoppingEndpointWithCancellationToken
var tokenSource = new CancellationTokenSource();
tokenSource.CancelAfter(TimeSpan.FromSeconds(1));
await host.StopAsync(tokenSource.Token);
#endregion



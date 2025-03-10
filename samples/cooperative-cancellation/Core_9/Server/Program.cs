using System;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Server;


var builder = Host.CreateApplicationBuilder(args);

Console.Title = "Cancellation";
var endpointConfiguration = new EndpointConfiguration("Samples.Cooperative.Cancellation");
endpointConfiguration.UsePersistence<LearningPersistence>();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

builder.UseNServiceBus(endpointConfiguration);
builder.Services.AddHostedService<InputLoopService>();

var host =  builder.Build();
var runTask = host.RunAsync();
Console.ReadKey();

var tokenSource = new CancellationTokenSource();
tokenSource.CancelAfter(TimeSpan.FromSeconds(1));

await host.StopAsync(tokenSource.Token);

await runTask;



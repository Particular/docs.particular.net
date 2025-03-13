using System;
using System.Threading.Tasks;
using Endpoint;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
var builder = Host.CreateApplicationBuilder(args);

Console.Title = "UnitOfWorkEndpoint";
builder.Services.AddHostedService<InputLoopService>();

var endpointConfiguration = new EndpointConfiguration("Samples.Pipeline.UnitOfWork.Endpoint");
endpointConfiguration.UsePersistence<LearningPersistence>();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

#region configuration
builder.Services.AddSingleton<MySessionProvider>();

// Then later get it from the service provider when needed
var serviceProvider = builder.Services.BuildServiceProvider();
var sessionProvider = serviceProvider.GetRequiredService<MySessionProvider>();


var pipeline = endpointConfiguration.Pipeline;
pipeline.Register(new MyUowBehavior(sessionProvider), "Manages the session");
#endregion



builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();

using System;
using Endpoint;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "UnitOfWorkEndpoint";
var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<InputLoopService>();
var endpointConfiguration = new EndpointConfiguration("Samples.Pipeline.UnitOfWork.Endpoint");
endpointConfiguration.UsePersistence<LearningPersistence>();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

#region configuration

var pipeline = endpointConfiguration.Pipeline;
pipeline.Register(new MyUowBehavior(), "Manages the session");
endpointConfiguration.RegisterComponents(c =>
{
    c.AddScoped<IMySession, MySession>();
    c.AddScoped<MySession, MySession>();
});
#endregion

Console.WriteLine("Press any key, the application is starting");
Console.ReadKey();
Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();


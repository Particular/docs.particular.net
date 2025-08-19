using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using SenderAndReceiver;

Console.Title = "SenderAndReceiver";

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<InputLoopService>();
var endpointConfiguration = new EndpointConfiguration("Samples.AzureDataBusCleanupWithFunctions.SenderAndReceiver");

#pragma warning disable CS0618 // Type or member is obsolete
var dataBus = endpointConfiguration.UseDataBus<AzureDataBus, SystemJsonDataBusSerializer>();
#pragma warning restore CS0618 // Type or member is obsolete
dataBus.ConnectionString("UseDevelopmentStorage=true");

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport<LearningTransport>();
endpointConfiguration.EnableInstallers();
builder.UseNServiceBus(endpointConfiguration);

await builder.Build().RunAsync();
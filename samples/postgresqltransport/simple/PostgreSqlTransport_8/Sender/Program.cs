using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Sender;

Console.Title = "SimpleSender";
var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<InputLoopService>();
var endpointConfiguration = new EndpointConfiguration("PostgreSql.SimpleSender");
endpointConfiguration.EnableInstallers();

#region TransportConfiguration
var connectionString = "User ID=user;Password=admin;Host=localhost;Port=54320;Database=nservicebus;Pooling=true;Connection Lifetime=0;Include Error Detail=true";
var routing = endpointConfiguration.UseTransport(new PostgreSqlTransport(connectionString)
{
    TransportTransactionMode = TransportTransactionMode.SendsAtomicWithReceive
});

routing.RouteToEndpoint(typeof(MyCommand), "PostgreSql.SimpleReceiver");

#endregion

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
builder.UseNServiceBus(endpointConfiguration);

await builder.Build().RunAsync();

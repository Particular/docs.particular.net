using System;
using Microsoft.Extensions.Hosting;
using NServiceBus;


Console.Title = "SimpleReceiver";
var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("PostgreSql.SimpleReceiver");

var connectionString = "User ID=user;Password=admin;Host=localhost;Port=54320;Database=nservicebus;Pooling=true;Connection Lifetime=0;Include Error Detail=true";

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new PostgreSqlTransport(connectionString)
{
    TransportTransactionMode = TransportTransactionMode.SendsAtomicWithReceive
});

endpointConfiguration.EnableInstallers();

Console.WriteLine("Waiting for message from the Sender");
builder.UseNServiceBus(endpointConfiguration);

await builder.Build().RunAsync();
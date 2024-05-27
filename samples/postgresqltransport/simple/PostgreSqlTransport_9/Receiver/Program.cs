using System;
using NServiceBus;


Console.Title = "SimpleReceiver";
var endpointConfiguration = new EndpointConfiguration("PostgreSql.SimpleReceiver");

var connectionString = "User ID=user;Password=admin;Host=localhost;Port=54320;Database=nservicebus;Pooling=true;Connection Lifetime=0;Include Error Detail=true";

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new PostgreSqlTransport(connectionString)
{
    TransportTransactionMode = TransportTransactionMode.SendsAtomicWithReceive
});

endpointConfiguration.EnableInstallers();

var endpointInstance = await Endpoint.Start(endpointConfiguration);
Console.WriteLine("Press any key to exit");
Console.WriteLine("Waiting for message from the Sender");
Console.ReadKey();
await endpointInstance.Stop();
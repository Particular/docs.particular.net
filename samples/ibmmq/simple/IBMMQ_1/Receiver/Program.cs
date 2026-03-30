using Microsoft.Extensions.Hosting;
using NServiceBus.Transport.IBMMQ;

Console.Title = "Receiver";
var builder = Host.CreateApplicationBuilder(args);

var ibmmq = new IBMMQTransport()
{
   QueueManagerName = "QM1",
   Host = "localhost",
   Port = 1414,
   Channel = "DEV.ADMIN.SVRCONN",
   User = "admin",
   Password = "passw0rd"
};

#region ReceiverConfig
var endpointB = new EndpointConfiguration("DEV.RECEIVER");
endpointB.UseTransport(ibmmq);
endpointB.UseSerialization<SystemJsonSerializer>();
endpointB.EnableInstallers();

// Delayed retries must be disabled as the IBM MQ transport does not support them
endpointB.Recoverability().Delayed(settings => settings.NumberOfRetries(0));
builder.UseNServiceBus(endpointB);
#endregion

var host = builder.Build();

await host.RunAsync();
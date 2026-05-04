using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Oracle.ManagedDataAccess.Client;

Console.Title = "EndpointOracle";

var endpointConfiguration = new EndpointConfiguration("EndpointOracle");

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.EnableInstallers();

var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();

var connection = "Data Source=localhost;User Id=SYSTEM; Password=yourStrong(!)Password; Enlist=false";

persistence.SqlDialect<SqlDialect.Oracle>();
persistence.ConnectionBuilder(() => new OracleConnection(connection));

var subscriptions = persistence.SubscriptionSettings();
subscriptions.CacheFor(TimeSpan.FromMinutes(1));

var builder = Host.CreateApplicationBuilder();
builder.Services.AddNServiceBusEndpoint(endpointConfiguration);
var host = builder.Build();
var messageSession = host.Services.GetRequiredService<IMessageSession>();
await host.StartAsync();
await SendMessage(messageSession);

Console.WriteLine("StartOrder Message sent");

Console.WriteLine("Press any key to exit");
Console.ReadKey();

await host.StopAsync();
using System;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.TransactionalSession;

Console.Title = "Frontend";
var builder = Host.CreateDefaultBuilder(args);
builder.UseConsoleLifetime();

builder.UseNServiceBus(builderContext =>
{
    var endpointConfiguration = new EndpointConfiguration("Samples.TransactionalSession.Frontend");
    endpointConfiguration.UseSerialization<SystemJsonSerializer>();
    endpointConfiguration.EnableInstallers();
    var transport = endpointConfiguration.UseTransport(new LearningTransport() { TransportTransactionMode = TransportTransactionMode.ReceiveOnly });

    #region cosmos-txsession-frontend-config
    endpointConfiguration.EnableOutbox();

    var persistence = endpointConfiguration.UsePersistence<CosmosPersistence>();
    persistence.CosmosClient(new CosmosClient(Configuration.CosmosDBConnectionString));
    persistence.DefaultContainer("Orders", "/CustomerId");

    persistence.EnableTransactionalSession();
    #endregion

    endpointConfiguration.PurgeOnStartup(true);

    return endpointConfiguration;
});

builder.ConfigureServices(services => services.AddHostedService<Worker>());

builder.Build().Run();
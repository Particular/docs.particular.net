using System;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

internal class Program
{
    public static void Main(string[] args)
    {
        Console.Title = "Frontend";
        var host = CreateHostBuilder(args).Build();
        host.Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args);
        builder.UseConsoleLifetime();

        builder.UseNServiceBus(builderContext =>
        {
            var endpointConfiguration = new EndpointConfiguration("Samples.TransactionalSession.Frontend");
            endpointConfiguration.EnableInstallers();
            var transport = endpointConfiguration.UseTransport(new LearningTransport() { TransportTransactionMode = TransportTransactionMode.ReceiveOnly });

            #region cosmos-txsession-frontend-config
            endpointConfiguration.EnableOutbox();
            endpointConfiguration.EnableTransactionalSession();
            #endregion

            #region cosmos-txsession-frontend-persistence
            var persistence = endpointConfiguration.UsePersistence<CosmosPersistence>();
            persistence.CosmosClient(new CosmosClient(Configuration.CosmosDBConnectionString));
            persistence.DefaultContainer("Orders", "/CustomerId");
            #endregion

            endpointConfiguration.PurgeOnStartup(true);

            return endpointConfiguration;
        });


        return builder.ConfigureServices(services => { services.AddHostedService<Worker>(); });
    }
}

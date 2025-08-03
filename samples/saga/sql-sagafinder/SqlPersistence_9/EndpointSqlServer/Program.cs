using System;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence.Sql;
using Microsoft.Extensions.Hosting;
using EndpointSqlServer;
using Microsoft.Extensions.DependencyInjection;


Console.Title = "SqlServer";
var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<InputLoopService>();

var endpointConfiguration = new EndpointConfiguration("Samples.SqlSagaFinder.SqlServer");
endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.UseSerialization<XmlSerializer>();
endpointConfiguration.EnableInstallers();

#region sqlServerConfig

//for local instance or SqlExpress
//var connectionString = @"Data Source=(localdb)\mssqllocaldb;Database=NsbSamplesSqlSagaFinder;Trusted_Connection=True;MultipleActiveResultSets=true";
var connectionString = @"Server=localhost,1433;Initial Catalog=NsbSamplesSqlSagaFinder;User Id=SA;Password=yourStrong(!)Password;Encrypt=false";
var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
persistence.SqlDialect<SqlDialect.MsSqlServer>();
persistence.ConnectionBuilder(
    connectionBuilder: () =>
    {
        return new SqlConnection(connectionString);
    });
var subscriptions = persistence.SubscriptionSettings();
subscriptions.CacheFor(TimeSpan.FromMinutes(1));

#endregion

await SqlHelper.EnsureDatabaseExists(connectionString);

builder.UseNServiceBus(endpointConfiguration);

await builder.Build().RunAsync();

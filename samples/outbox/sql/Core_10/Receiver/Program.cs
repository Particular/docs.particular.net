using System;
using System.IO;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Transport.SqlServer;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) => { Console.Title = "Server"; })
    .UseNServiceBus(x =>
    {
        Console.Title = "Receiver";

        //for local instance or SqlExpress
        //string connectionString = @"Data Source=(localdb)\mssqllocaldb;Database=NsbSamplesSqlOutbox;Trusted_Connection=True;MultipleActiveResultSets=true";
        const string connectionString = @"Server=localhost,1433;Initial Catalog=NsbSamplesSqlOutbox;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false";

        var endpointConfiguration = new EndpointConfiguration("Samples.SqlOutbox.Receiver");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        #region ReceiverConfiguration

        var transport = new SqlServerTransport(connectionString)
        {
            DefaultSchema = "receiver",
            TransportTransactionMode = TransportTransactionMode.ReceiveOnly
        };
        transport.SchemaAndCatalog.UseSchemaForQueue("error", "dbo");
        transport.SchemaAndCatalog.UseSchemaForQueue("audit", "dbo");

        var routing = endpointConfiguration.UseTransport(transport);
        routing.UseSchemaForEndpoint("Samples.SqlOutbox.Sender", "sender");

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.ConnectionBuilder(
            connectionBuilder: () => new SqlConnection(connectionString)
        );
        var dialect = persistence.SqlDialect<SqlDialect.MsSqlServer>();
        dialect.Schema("receiver");
        persistence.TablePrefix("");

        transport.Subscriptions.DisableCaching = true;
        transport.Subscriptions.SubscriptionTableName = new SubscriptionTableName(
            table: "Subscriptions",
            schema: "dbo"
        );

        endpointConfiguration.EnableOutbox();

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        #endregion

        SqlHelper.CreateSchema(connectionString, "receiver");

        SqlHelper.ExecuteSql(connectionString, File.ReadAllText("Startup.sql"));
        return endpointConfiguration;
    })
    .Build();


await host.RunAsync();
﻿//#define POST_MIGRATION

using System;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.SagaMigration.Server.New";
        var endpointConfiguration = new EndpointConfiguration("Samples.SagaMigration.Server");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();

#if !POST_MIGRATION
        endpointConfiguration.OverrideLocalAddress("Samples.SagaMigration.Server.New");
#endif

        endpointConfiguration.UseTransport(new LearningTransport());
        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.SqlDialect<SqlDialect.MsSqlServer>();

        // for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=NsbSamplesSagaMigration;Integrated Security=True;Encrypt=false
        var connectionString = @"Server=localhost,1433;Initial Catalog=NsbSamplesSagaMigration;User Id=SA;Password=yourStrong(!)Password;Encrypt=false";

        persistence.ConnectionBuilder(
            connectionBuilder: () =>
            {
                return new SqlConnection(connectionString);
            });
        persistence.TablePrefix("New");

        SqlHelper.EnsureDatabaseExists(connectionString);
        var endpoint = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press <enter> to exit.");
        Console.ReadLine();

        await endpoint.Stop()
            .ConfigureAwait(false);
    }
}

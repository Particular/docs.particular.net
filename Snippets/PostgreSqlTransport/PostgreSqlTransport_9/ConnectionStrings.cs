using System;
using Azure.Core;
using Azure.Identity;
using Npgsql;
using NServiceBus;

class ConnectionStrings
{
    void ConnectionString(EndpointConfiguration endpointConfiguration)
    {
        #region postgresql-config-connectionstring

        var transport = new PostgreSqlTransport("SomeConnectionString");

        #endregion
    }

    void ConnectionEntra(EndpointConfiguration endpointConfiguration)
    {
        #region postgresql-config-entra

        var connection = "Server=test.postgres.database.azure.com;Database=postgres;Port=5432;User Id=<entra user id>;Ssl Mode=Require;";
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connection);
        if (string.IsNullOrEmpty(dataSourceBuilder.ConnectionStringBuilder.Password))
        {
            dataSourceBuilder.UsePeriodicPasswordProvider(async (_, ct) =>
            {
                var credentials = new DefaultAzureCredential();
                var token = await credentials.GetTokenAsync(new TokenRequestContext(["https://ossrdbms-aad.database.windows.net/.default"]), ct);
                return token.Token;
            }, TimeSpan.FromHours(24), TimeSpan.FromSeconds(10));
        }
        var builder = dataSourceBuilder.Build();
        var transport = new PostgreSqlTransport(async cancellationToken =>
        {
            var connection = builder.CreateConnection();
            try
            {
                await connection.OpenAsync(cancellationToken);
                return connection;
            }
            catch
            {
                connection.Dispose();
                throw;
            }
        });

        #endregion
    }

    void ConnectionFactory(EndpointConfiguration endpointConfiguration)
    {
        #region postgresql-custom-connection-factory

        var transport = new PostgreSqlTransport(
            async cancellationToken =>
            {
                var connection = new NpgsqlConnection("SomeConnectionString");
                try
                {
                    await connection.OpenAsync(cancellationToken);

                    // perform custom operations

                    return connection;
                }
                catch
                {
                    connection.Dispose();
                    throw;
                }
            });

        #endregion
    }
}
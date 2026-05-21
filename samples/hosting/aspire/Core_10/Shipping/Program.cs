using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Npgsql;
using NpgsqlTypes;

var builder = Host.CreateApplicationBuilder();

builder.AddServiceDefaults()
    .AddNServiceBusEndpoint("Shipping", (endpointConfiguration, routing) =>
    {
        #region persistence-config
        var persistenceConnection = builder.Configuration.GetConnectionString("shipping-db");
        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.ConnectionBuilder(
            connectionBuilder: () => new NpgsqlConnection(persistenceConnection));
        #endregion

        var dialect = persistence.SqlDialect<SqlDialect.PostgreSql>();
        dialect.JsonBParameterModifier(
            modifier: parameter =>
            {
                var npgsqlParameter = (NpgsqlParameter)parameter;
                npgsqlParameter.NpgsqlDbType = NpgsqlDbType.Jsonb;
            });

        endpointConfiguration.AuditSagaStateChanges(serviceControlQueue: "audit");
    });

await builder.Build().RunAsync();

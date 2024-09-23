using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Npgsql;
using NpgsqlTypes;
using NServiceBus;
using System.Threading.Tasks;

namespace Shipping
{
    class Program
    {
        static Task Main()
        {
            #region always-config
            var builder = Host.CreateApplicationBuilder();

            var endpointConfiguration = new EndpointConfiguration("Shipping");
            endpointConfiguration.EnableOpenTelemetry();

            builder.AddServiceDefaults();
            #endregion

            #region transport-config
            var connectionString = builder.Configuration.GetConnectionString("transport");
            var transport = new RabbitMQTransport(RoutingTopology.Conventional(QueueType.Quorum), connectionString);
            #endregion

            var routing = endpointConfiguration.UseTransport(transport);

            #region persistence-config
            var persistenceConnection = builder.Configuration.GetConnectionString("shipping-db");
            var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
            persistence.ConnectionBuilder(
                connectionBuilder: () =>
                {
                    return new NpgsqlConnection(persistenceConnection);
                });
            #endregion

            var dialect = persistence.SqlDialect<SqlDialect.PostgreSql>();
            dialect.JsonBParameterModifier(
                modifier: parameter =>
                {
                    var npgsqlParameter = (NpgsqlParameter)parameter;
                    npgsqlParameter.NpgsqlDbType = NpgsqlDbType.Jsonb;
                });

            endpointConfiguration.UseSerialization<SystemJsonSerializer>();

            #region enable-installers
            endpointConfiguration.EnableInstallers();
            #endregion

            builder.UseNServiceBus(endpointConfiguration);

            return builder.Build().RunAsync();
        }
    }
}
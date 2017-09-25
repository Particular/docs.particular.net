using System.Data.SqlClient;
using NServiceBus;
using NServiceBus.Transport.SQLServer;

class LegacyMultiInstance
{
#pragma warning disable 0618

    void ConfigureLegacyMultiInstance(EndpointConfiguration endpointConfiguration)
    {
        #region 31to4-legacy-multi-instance

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.EnableLegacyMultiInstanceMode(async address =>
        {
            var connectionString = address.StartsWith("RemoteEndpoint")
                ? "Data Source=SQL; Database=RemoteEndpoint; Integrated Security=True"
                : "Data Source=SQL; Database=ThisEndpoint; Integrated Security=True";
            var connection = new SqlConnection(connectionString);
            await connection.OpenAsync()
                .ConfigureAwait(false);
            return connection;
        });

        #endregion
    }

#pragma warning restore 0618
}
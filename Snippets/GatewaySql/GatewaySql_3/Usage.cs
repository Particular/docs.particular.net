using NServiceBus;
using Microsoft.Data.SqlClient;

class Usage
{
    public void StandardUsage(EndpointConfiguration endpointConfiguration, string connectionString)
    {
        #region DefaultUsage
        var gatewayConfiguration = new SqlGatewayDeduplicationConfiguration();
        gatewayConfiguration.ConnectionBuilder(
            connectionBuilder: () =>
            {
                return new SqlConnection(connectionString);
            });

        var gatewaySettings = endpointConfiguration.Gateway(gatewayConfiguration);
        #endregion
    }

    public void SchemaAndTableName(EndpointConfiguration endpointConfiguration, string connectionString)
    {
        #region CustomizeSchemaAndTableName
        var gatewayConfiguration = new SqlGatewayDeduplicationConfiguration();
        gatewayConfiguration.Schema = "custom_schema";
        gatewayConfiguration.TableName = "CustomTableName";
        gatewayConfiguration.ConnectionBuilder(
            connectionBuilder: () =>
            {
                return new SqlConnection(connectionString);
            });

        var gatewaySettings = endpointConfiguration.Gateway(gatewayConfiguration);
        #endregion
    }

    public void WithEndpointName(SqlGatewayDeduplicationConfiguration gatewayConfiguration, string endpointName)
    {
        #region WithEndpointName
        gatewayConfiguration.TableName = $"{endpointName}_GatewayDeduplication";
        #endregion
    }
}
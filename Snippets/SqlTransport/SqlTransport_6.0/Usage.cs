using NServiceBus;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region Usage

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString("connectionString");

        #endregion

        #region MessageBodyString-config

        var transportConfig = endpointConfiguration.UseTransport<SqlServerTransport>();
        transportConfig.CreateMessageBodyComputedColumn();

        #endregion
    }
}
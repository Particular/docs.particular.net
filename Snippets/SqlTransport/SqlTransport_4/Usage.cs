using NServiceBus;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region Usage

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString("connectionString");

        #endregion
    }
}
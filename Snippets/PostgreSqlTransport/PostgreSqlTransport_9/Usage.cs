using NServiceBus;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region Usage

        endpointConfiguration.UseTransport(new PostgreSqlTransport("connectionString"));

        #endregion
    }
}
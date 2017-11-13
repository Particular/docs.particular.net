using NServiceBus;

class Usage
{
    Usage(BusConfiguration busConfiguration)
    {
        #region Usage

        var transport = busConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString("connectionString");

        #endregion
    }
}
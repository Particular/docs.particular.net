using NServiceBus;

class NamedConnectionString
{
    void ConnectionName(Configure configure)
    {
        #region sqlserver-named-connection-string

        var transport = configure.UseTransport<SqlServer>("MyConnectionString");

        #endregion
    }
}
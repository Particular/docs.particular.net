namespace Snippets5.Transports.SqlServer
{
    using NServiceBus;

    public class NamedConnectionString
    {
        void ConnectionName()
        {
            #region sqlserver-named-connection-string 2

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseTransport<SqlServerTransport>()
                .ConnectionStringName("MyConnectionString");

            #endregion
        }

    }
}
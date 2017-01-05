namespace Core_6
{
    using NServiceBus;

    class SqlTransportOption
    {
        static void ConfigureSqlTransport(EndpointConfiguration endpointConfiguration)
        {
            #region SqlServerTransport
            var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
            transport.ConnectionString(@"Server=.\sqlexpress;Initial Catalog=NServiceBusAcademy;Trusted_Connection=true;Max Pool Size=100;");
            #endregion
        }
    }
}

using NServiceBus;

class MultiSchema
{
    void NonStandardSchemaInConnectionString(BusConfiguration busConfiguration)
    {
        #region sqlserver-non-standard-schema-connString

        var transport = busConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString("Data Source=INSTANCE_NAME;Initial Catalog=some_database; Queue Schema=myschema");

        #endregion
    }
}
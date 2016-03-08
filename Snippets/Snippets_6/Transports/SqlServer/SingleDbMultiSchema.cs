namespace Snippets5.Transports.SqlServer
{
    using NServiceBus;
    using NServiceBus.Transports.SQLServer;

    public class SingleDbMultiSchema
    {
        void CurrentEndpointSchema()
        {
            #region sqlserver-singledb-multischema 3

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.UseTransport<SqlServerTransport>()
                .DefaultSchema("myschema");

            #endregion
        }

        void CurrentEndpointSchemaInConnString()
        {
            #region sqlserver-singledb-multischema-connString 3

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.UseTransport<SqlServerTransport>()
                .ConnectionString("Data Source=INSTANCE_NAME; Initial Catalog=some_database; Integrated Security=True; Queue Schema=nsb");

            #endregion
        }

        void OtherEndpointConnectionParamsPull()
        {
            #region sqlserver-singledb-multidb-pull 3

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.UseTransport<SqlServerTransport>()
                .UseSpecificSchema(tn => tn == "AnotherEndpoint" ? "receiver1" : null);

            #endregion
        }
    }
}

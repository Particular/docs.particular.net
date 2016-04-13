namespace Snippets5.Transports.SqlServer
{
    using NServiceBus;
    using NServiceBus.Transports.SQLServer;

    class SingleDbMultiSchema
    {
        void CurrentEndpointSchema(EndpointConfiguration endpointConfiguration)
        {
            #region sqlserver-singledb-multischema

            endpointConfiguration.UseTransport<SqlServerTransport>()
                .DefaultSchema("myschema");

            #endregion
        }

        void OtherEndpointConnectionParamsPull(EndpointConfiguration endpointConfiguration)
        {
            #region sqlserver-singledb-multidb-pull

            endpointConfiguration.UseTransport<SqlServerTransport>()
                .UseSpecificSchema(tn => tn == "AnotherEndpoint" ? "receiver1" : null);

            #endregion
        }
    }
}

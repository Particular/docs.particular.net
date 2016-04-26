namespace SqlServer_3
{
    using NServiceBus;
    using NServiceBus.Transports.SQLServer;

    class SingleDbMultiSchema
    {
        void CurrentEndpointSchema(EndpointConfiguration endpointConfiguration)
        {
            #region sqlserver-singledb-multischema

            var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
            transport.DefaultSchema("myschema");

            #endregion
        }

        void OtherEndpointConnectionParamsPull(EndpointConfiguration endpointConfiguration)
        {
            #region sqlserver-singledb-multidb-pull

            var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
            transport.UseSpecificSchema(tn => tn == "AnotherEndpoint" ? "receiver1" : null);

            #endregion
        }
    }
}

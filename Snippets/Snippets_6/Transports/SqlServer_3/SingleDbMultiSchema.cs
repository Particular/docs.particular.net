namespace Snippets5.Transports.SqlServer
{
    using NServiceBus;
    using NServiceBus.Transports.SQLServer;

    public class SingleDbMultiSchema
    {
        void CurrentEndpointSchema()
        {
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            #region sqlserver-singledb-multischema

            endpointConfiguration.UseTransport<SqlServerTransport>()
                .DefaultSchema("myschema");

            #endregion
        }

        void OtherEndpointConnectionParamsPull()
        {
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            #region sqlserver-singledb-multidb-pull

            endpointConfiguration.UseTransport<SqlServerTransport>()
                .UseSpecificSchema(tn => tn == "AnotherEndpoint" ? "receiver1" : null);

            #endregion
        }
    }
}

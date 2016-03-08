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

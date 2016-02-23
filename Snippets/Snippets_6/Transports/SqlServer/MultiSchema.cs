namespace Snippets5.Transports.SqlServer
{
    using NServiceBus;
    using NServiceBus.Transports.SQLServer;

    public class SingleDbMultiSchema
    {
        void CurrentEndpointSchema()
        {
            BusConfiguration busConfiguration = new BusConfiguration();

            #region sqlserver-singledb-multischema 3

            busConfiguration.UseTransport<SqlServerTransport>()
                .DefaultSchema("myschema");

            #endregion
        }
		

        void OtherEndpointConnectionParamsPull()
        {
            BusConfiguration busConfiguration = new BusConfiguration();

            #region sqlserver-singledb-multidb-pull 3

            busConfiguration.UseTransport<SqlServerTransport>()
							.UseSpecificSchema(tn => tn == EndpointNamingConvention(typeof(AnotherEndpoint)) ? "receiver1" : null);

            #endregion
        }
    }
}
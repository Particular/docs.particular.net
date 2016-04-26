namespace SqlServer_2
{
    using NServiceBus;
    using NServiceBus.Transports.SQLServer;

    class SingleDbMultiSchema
    {
        void CurrentEndpointSchema(BusConfiguration busConfiguration)
        {
            #region sqlserver-singledb-multischema 2.1

            var transport = busConfiguration.UseTransport<SqlServerTransport>();
            transport.DefaultSchema("myschema");

            #endregion
        }


        void OtherEndpointConnectionParamsPull(BusConfiguration busConfiguration)
        {
            #region sqlserver-singledb-multidb-pull 2.1

            var transport = busConfiguration.UseTransport<SqlServerTransport>();
            transport.UseSpecificConnectionInformation(x => x == "AnotherEndpoint"
                    ? ConnectionInfo.Create()
                        .UseSchema("nsb")
                    : null);

            #endregion
        }
    }
}

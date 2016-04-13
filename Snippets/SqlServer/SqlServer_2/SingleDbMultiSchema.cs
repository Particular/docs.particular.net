namespace SqlServer_2
{
    using NServiceBus;
    using NServiceBus.Transports.SQLServer;

    class SingleDbMultiSchema
    {
        void CurrentEndpointSchema(BusConfiguration busConfiguration)
        {
            #region sqlserver-singledb-multischema 2.1

            busConfiguration.UseTransport<SqlServerTransport>()
                .DefaultSchema("myschema");

            #endregion
        }


        void OtherEndpointConnectionParamsPull(BusConfiguration busConfiguration)
        {
            #region sqlserver-singledb-multidb-pull 2.1

            busConfiguration.UseTransport<SqlServerTransport>()
                .UseSpecificConnectionInformation(x => x == "AnotherEndpoint"
                    ? ConnectionInfo.Create()
                        .UseSchema("nsb")
                    : null);

            #endregion
        }
    }
}

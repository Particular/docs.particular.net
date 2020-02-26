using NServiceBus;
using NServiceBus.Transport.SQLServer;

namespace SqlTransport_3.UpgradeGuides._2to3
{
    class MultiSchema
    {
        void NonStandardSchema(EndpointConfiguration endpointConfiguration)
        {
            #region 2to3-sqlserver-non-standard-schema

            var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
            transport.DefaultSchema("myschema");

            #endregion
        }

    }
}
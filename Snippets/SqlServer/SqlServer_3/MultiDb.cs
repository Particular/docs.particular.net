using System;
using System.Data.SqlClient;
using NServiceBus;
using NServiceBus.Transport.SQLServer;

class MultiDb
{
#pragma warning disable 0618

    void OtherEndpointConnectionParamsPull(EndpointConfiguration endpointConfiguration)
    {
        #region sqlserver-multidb-other-endpoint-connection

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.EnableLegacyMultiInstanceMode(async address =>
        {
            var connectionString = address.Equals("RemoteEndpoint") ? "SomeConnectionString" : "SomeOtherConnectionString";
            var connection = new SqlConnection(connectionString);
            await connection.OpenAsync()
                .ConfigureAwait(false);
            return connection;
        });

        #endregion
    }

    void ServiceControlMultiInstanceEndpointConnectionStrings(EndpointConfiguration endpointConfiguration)
    {
        #region sc-multi-instance-endpoint-connection-strings

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.EnableLegacyMultiInstanceMode(async transportAddress =>
        {
            string connectionString;

            if (transportAddress == "error" ||
                transportAddress == "audit" ||
                transportAddress.StartsWith("Particular.ServiceControl"))
            {
                connectionString = "Server=DbServerA;Database=ServiceControlDB;";
            }
            else if (transportAddress == "Billing")
            {
                connectionString = "Server=DbServerB;Database=BillingDB;";
            }
            else if (transportAddress == "Sales")
            {
                connectionString = "Server=DbServerC;Database=SalesDB;";
            }
            else
            {
                throw new Exception($"Connection string not found for transport address {transportAddress}");
            }

            var connection = new SqlConnection(connectionString);
            await connection.OpenAsync()
                .ConfigureAwait(false);
            return connection;
        });

        #endregion
    }

#pragma warning restore 0618
}
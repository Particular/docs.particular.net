using System;
using System.Data.SqlClient;

namespace SqlServer_All.Operations.QueueCreation
{
    public static class CreateEndpointQueues
    {

        static void CreateQueuesForEndpoint()
        {
            var connectionString = @"Data Source=.\SqlExpress;Database=samples;Integrated Security=True";

            #region create-queues-endpoint-usage

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                CreateQueuesForEndpoint(
                        connection: connection,
                        schema: "dbo",
                        endpointName: "myendpoint");
            }

            #endregion
        }
        #region create-queues-for-endpoint

        public static void CreateQueuesForEndpoint(SqlConnection connection, string schema, string endpointName)
        {
            // main queue
            QueueCreationUtils.CreateQueue(connection, schema, endpointName);

            // callback queue
            QueueCreationUtils.CreateQueue(connection, schema, $"{endpointName}.{Environment.MachineName}");

            // timeout queue
            QueueCreationUtils.CreateQueue(connection, schema, $"{endpointName}.Timeouts");

            // timeout dispatcher queue
            QueueCreationUtils.CreateQueue(connection, schema, $"{endpointName}.TimeoutsDispatcher");

            // retries queue
            // TODO: Only required in Versions 2 and below
            QueueCreationUtils.CreateQueue(connection, schema, $"{endpointName}.Retries");
        }

        #endregion
    }
}
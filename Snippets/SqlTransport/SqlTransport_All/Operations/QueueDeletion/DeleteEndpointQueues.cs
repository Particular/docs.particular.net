using System;
using System.Data.SqlClient;

namespace SqlServer_All.Operations.QueueDeletion
{
    public static class DeleteEndpointQueues
    {
        static void DeleteQueuesForEndpoint()
        {
            var connectionString = @"Data Source=.\SqlExpress;Database=samples;Integrated Security=True";

            #region sqlserver-delete-queues-endpoint-usage

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                DeleteQueuesForEndpoint(
                    connection: sqlConnection,
                    schema: "dbo",
                    endpointName: "myendpoint");
            }

            #endregion
        }

        #region sqlserver-delete-queues-for-endpoint

        public static void DeleteQueuesForEndpoint(SqlConnection connection, string schema, string endpointName)
        {
            // main queue
            QueueDeletionUtils.DeleteQueue(connection, schema, endpointName);

            // callback queue
            QueueDeletionUtils.DeleteQueue(connection, schema, $"{endpointName}.{Environment.MachineName}");

            // timeout queue
            QueueDeletionUtils.DeleteQueue(connection, schema, $"{endpointName}.Timeouts");

            // timeout dispatcher queue
            QueueDeletionUtils.DeleteQueue(connection, schema, $"{endpointName}.TimeoutsDispatcher");

            // retries queue
            // TODO: Only required in Versions 2 and below
            QueueDeletionUtils.DeleteQueue(connection, schema, $"{endpointName}.Retries");
        }

        #endregion
    }
}
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SqlServer_All.Operations.QueueDeletion
{
    public static class DeleteEndpointQueues
    {
        static async Task DeleteQueuesForEndpoint()
        {
            var connectionString = @"Data Source=.\SqlExpress;Database=samples;Integrated Security=True";

            #region sqlserver-delete-queues-endpoint-usage

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync()
                    .ConfigureAwait(false);
                await DeleteQueuesForEndpoint(
                        connection: sqlConnection,
                        schema: "dbo",
                        endpointName: "myendpoint")
                    .ConfigureAwait(false);
            }

            #endregion
        }

        #region sqlserver-delete-queues-for-endpoint

        public static async Task DeleteQueuesForEndpoint(SqlConnection connection, string schema, string endpointName)
        {
            // main queue
            await QueueDeletionUtils.DeleteQueue(connection, schema, endpointName)
                .ConfigureAwait(false);

            // callback queue
            await QueueDeletionUtils.DeleteQueue(connection, schema, $"{endpointName}.{Environment.MachineName}")
                .ConfigureAwait(false);

            // timeout queue
            await QueueDeletionUtils.DeleteQueue(connection, schema, $"{endpointName}.Timeouts")
                .ConfigureAwait(false);

            // timeout dispatcher queue
            await QueueDeletionUtils.DeleteQueue(connection, schema, $"{endpointName}.TimeoutsDispatcher")
                .ConfigureAwait(false);

            // retries queue
            // TODO: Only required in Versions 2 and below
            await QueueDeletionUtils.DeleteQueue(connection, schema, $"{endpointName}.Retries")
                .ConfigureAwait(false);
        }

        #endregion
    }
}
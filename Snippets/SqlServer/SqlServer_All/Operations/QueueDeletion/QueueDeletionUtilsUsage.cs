using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SqlServer_All.Operations.QueueDeletion
{
    public static class QueueDeletionUtilsUsage
    {
        public static async Task DeleteQueuesForEndpoint()
        {
            var connectionString = @"Data Source=.\SqlExpress;Database=samples;Integrated Security=True";

            #region sqlserver-delete-queues-endpoint-usage

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync()
                    .ConfigureAwait(false);
                await QueueDeletionUtils.DeleteQueuesForEndpoint(
                        connection: sqlConnection,
                        schema: "dbo",
                        endpointName: "myendpoint")
                    .ConfigureAwait(false);
            }

            #endregion
        }

        public static async Task DeleteSharedQueues()
        {
            var connectionString = @"Data Source=.\SqlExpress;Database=samples;Integrated Security=True";

            #region sqlserver-delete-queues-shared-usage

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync()
                    .ConfigureAwait(false);
                await QueueDeletionUtils.DeleteQueue(
                        connection: sqlConnection,
                        schema: "dbo",
                        queueName: "audit")
                    .ConfigureAwait(false);
                await QueueDeletionUtils.DeleteQueue(
                        connection: sqlConnection,
                        schema: "dbo",
                        queueName: "error")
                    .ConfigureAwait(false);
            }

            #endregion
        }

    }
}
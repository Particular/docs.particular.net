using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SqlServer_All.Operations.QueueDeletion
{
    public static class DeleteEndpointQueues_3_and_above
    {

        #region sqlserver-delete-queues-for-endpoint [3,]

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
        }

        #endregion
    }
}
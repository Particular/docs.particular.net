using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

#region sqlserver-delete-queues
namespace SqlServer_All.Operations.QueueDeletion
{
    public static class QueueDeletionUtils
    {

        public static async Task DeleteQueue(SqlConnection connection, string schema, string queueName)
        {
            var deleteScript = $@"
                IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[{schema}].[{queueName}]') AND type in (N'U'))
                DROP TABLE [{schema}].[{queueName}]";
            using (var command = new SqlCommand(deleteScript, connection))
            {
                await command.ExecuteNonQueryAsync()
                    .ConfigureAwait(false);
            }
        }

        public static async Task DeleteQueuesForEndpoint(SqlConnection connection, string schema, string endpointName)
        {
            // main queue
            await DeleteQueue(connection, schema, endpointName)
                .ConfigureAwait(false);

            // callback queue
            await DeleteQueue(connection, schema, $"{endpointName}.{Environment.MachineName}")
                .ConfigureAwait(false);

            // retries queue
            await DeleteQueue(connection, schema, $"{endpointName}.Retries")
                .ConfigureAwait(false);

            // timeout queue
            await DeleteQueue(connection, schema, $"{endpointName}.Timeouts")
                .ConfigureAwait(false);

            // timeout dispatcher queue
            await DeleteQueue(connection, schema, $"{endpointName}.TimeoutsDispatcher")
                .ConfigureAwait(false);
        }

    }
}
#endregion
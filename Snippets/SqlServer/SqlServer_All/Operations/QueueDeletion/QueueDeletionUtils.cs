using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SqlServer_All.Operations.QueueDeletion
{

    #region sqlserver-delete-queues

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
    }

    #endregion
}
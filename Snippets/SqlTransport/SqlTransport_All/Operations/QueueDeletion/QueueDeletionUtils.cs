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
                if exists (select * from sys.objects where object_id = object_id(N'[{schema}].[{queueName}]') and type in (N'U'))
                drop table [{schema}].[{queueName}]";
            using (var command = new SqlCommand(deleteScript, connection))
            {
                await command.ExecuteNonQueryAsync()
                    .ConfigureAwait(false);
            }
        }
    }

    #endregion
}
using System.Data.SqlClient;

namespace SqlServer_All.Operations.QueueDeletion
{
    public static class QueueDeletionUtilsUsage
    {

        public static void DeleteSharedQueues()
        {
            var connectionString = @"Data Source=.\SqlExpress;Database=samples;Integrated Security=True";

            #region sqlserver-delete-queues-shared-usage

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                QueueDeletionUtils.DeleteQueue(
                    connection: connection,
                    schema: "dbo",
                    queueName: "audit");
                QueueDeletionUtils.DeleteQueue(
                    connection: connection,
                    schema: "dbo",
                    queueName: "error");
            }

            #endregion
        }

    }
}
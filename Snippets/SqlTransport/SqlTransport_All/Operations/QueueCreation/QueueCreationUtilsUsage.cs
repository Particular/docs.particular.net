using System.Data.SqlClient;

namespace SqlServer_All.Operations.QueueCreation
{
    public static class QueueCreationUtilsUsage
    {
        public static void CreateQueuesForEndpoint()
        {
            var connectionString = @"Data Source=.\SqlExpress;Database=samples;Integrated Security=True";

            #region create-queues-shared-usage

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                QueueCreationUtils.CreateQueue(
                    connection: sqlConnection,
                    schema: "dbo",
                    queueName: "error");

                QueueCreationUtils.CreateQueue(
                    connection: sqlConnection,
                    schema: "dbo",
                    queueName: "audit");
            }

            #endregion
        }

    }
}
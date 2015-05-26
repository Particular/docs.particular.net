namespace Operations.SqlServer
{
    using System.Data.SqlClient;

    public class QueueDeletionUsage
    {
        public void DeleteQueuesForEndpoint()
        {
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=myDatabase;Integrated Security=True";
            #region sqlserver-delete-queues-endpoint-usage
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                QueueDeletion.DeleteQueuesForEndpoint(
                    connection: sqlConnection,
                    schema: "dbo",
                    endpointName: "myendpoint");
            }

            #endregion
        }

        public void DeleteSharedQueues()
        {
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=myDatabase;Integrated Security=True";
            #region sqlserver-delete-queues-shared-usage
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                QueueDeletion.DeleteQueue(
                    connection: sqlConnection,
                    schema: "dbo",
                    queueName: "audit");
                QueueDeletion.DeleteQueue(
                    connection: sqlConnection,
                    schema: "dbo",
                    queueName: "error");
            }

            #endregion
        }
    }

}
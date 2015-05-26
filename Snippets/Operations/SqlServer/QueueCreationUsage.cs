namespace Operations.SqlServer
{
    using System.Data.SqlClient;

    public class QueueCreationUsage
    {
        public void CreateQueuesForEndpoint()
        {
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=myDatabase;Integrated Security=True";
            #region sqlserver-create-queues-endpoint-usage
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                QueueCreation.CreateQueuesForEndpoint(
                    connection: sqlConnection,
                    schema: "dbo",
                    endpointName: "myendpoint");
            }

            #endregion
        }

        public void CreateSharedQueues()
        {
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=myDatabase;Integrated Security=True";
            #region sqlserver-create-queues-shared-usage
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                QueueCreation.CreateQueue(
                    connection: sqlConnection,
                    schema: "dbo",
                    queueName: "error");

                QueueCreation.CreateQueue(
                    connection: sqlConnection,
                    schema: "dbo",
                    queueName: "audit");
            }

            #endregion
        }

    }

}
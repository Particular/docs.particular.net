namespace Operations.SqlServer.Tests
{
    using System.Data.SqlClient;
    using NUnit.Framework;

    [TestFixture]
    [Explicit]
    public class QueueCreationTests
    {
        [Test]
        public void CreateQueuesForEndpoint()
        {
            #region sqlserver-create-queues-endpoint-usage
            string databaseName = "myDatabase";
            string connectionString = string.Format(@"Data Source=.\SQLEXPRESS;Initial Catalog={0};Integrated Security=True", databaseName);
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                QueueCreation.CreateQueuesForEndpoint(
                    connection: sqlConnection,
                    schema: "dbo",
                    endpointName: "myendpoint");

                #endregion

                #region sqlserver-create-queues-shared-usage
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

        [Test]
        public void DeleteQueuesForEndpoint()
        {
            #region sqlserver-delete-queues-endpoint-usage
            string databaseName = "myDatabase";
            string connectionString = string.Format(@"Data Source=.\SQLEXPRESS;Initial Catalog={0};Integrated Security=True", databaseName);
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                QueueCreation.DeleteQueuesForEndpoint(
                    connection: sqlConnection,
                    schema: "dbo",
                    endpointName: "myendpoint");
            }

            #endregion
        }
        [Test]
        public void DeleteSharedQueues()
        {
            #region sqlserver-delete-queues-shared-usage
            string databaseName = "myDatabase";
            string connectionString = string.Format(@"Data Source=.\SQLEXPRESS;Initial Catalog={0};Integrated Security=True", databaseName);
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                QueueCreation.DeleteQueue(
                    connection: sqlConnection,
                    schema: "dbo",
                    queueName: "audit");
                QueueCreation.DeleteQueue(
                    connection: sqlConnection,
                    schema: "dbo",
                    queueName: "error");
            }

            #endregion
        }
    }

}
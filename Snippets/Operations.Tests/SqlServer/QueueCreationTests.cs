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
            string databaseName = "shared";
            string connectionString = string.Format(@"Data Source=.\SQLEXPRESS;Initial Catalog={0};Integrated Security=True", databaseName);
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            QueueCreation.CreateQueuesForEndpoint(
                connection: sqlConnection,
                schema: "dbo",
                endpointName: "myendpoint");

            QueueCreation.CreateQueue(
                connection: sqlConnection, 
                schema: "dbo",
                queueName: "error");

            QueueCreation.CreateQueue(
                connection: sqlConnection,
                schema: "dbo",
                queueName: "audit");
        }

        [Test]
        public void DeleteQueuesForEndpoint()
        {
            string databaseName = "shared";
            string connectionString = string.Format(@"Data Source=.\SQLEXPRESS;Initial Catalog={0};Integrated Security=True", databaseName);
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            QueueCreation.DeleteQueuesForEndpoint(
                connection: sqlConnection,
                schema: "dbo",
                endpointName: "myendpoint");
        }
    }

}
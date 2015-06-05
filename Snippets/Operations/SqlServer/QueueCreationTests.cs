namespace Operations.SqlServer
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
            string databaseName = "myDatabase";
            string connectionString = string.Format(@"Data Source=.\SQLEXPRESS;Initial Catalog={0};Integrated Security=True", databaseName);
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
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

        }

    }

}
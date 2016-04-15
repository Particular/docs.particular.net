namespace Operations.SqlServer
{
    using System.Data.SqlClient;
    using NUnit.Framework;

    [TestFixture]
    [Explicit]
    public class QueueDeletionTests
    {

        [Test]
        public void DeleteQueuesForEndpoint()
        {
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=samples;Integrated Security=True";
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                QueueDeletion.DeleteQueuesForEndpoint(
                    connection: sqlConnection,
                    schema: "dbo",
                    endpointName: "myendpoint");
            }
        }

        [Test]
        public void DeleteSharedQueues()
        {
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=samples;Integrated Security=True";
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

        }
    }

}
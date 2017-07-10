using System.Data.SqlClient;
using NUnit.Framework;

namespace SqlServer_All.Operations.QueueDeletion
{
    [TestFixture]
    [Explicit]
    public class Tests
    {

        [Test]
        public void DeleteQueuesForEndpoint()
        {
            var connectionString = @"Data Source=.\SqlExpress;Database=Snippets.SqlTransport;Integrated Security=True";
            SqlHelper.EnsureDatabaseExists(connectionString);
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                DeleteEndpointQueues.DeleteQueuesForEndpoint(
                    connection: sqlConnection,
                    schema: "dbo",
                    endpointName: "myendpoint");
            }
        }

        [Test]
        public void DeleteSharedQueues()
        {
            var connectionString = @"Data Source=.\SqlExpress;Database=samples;Integrated Security=True";
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                QueueDeletionUtils.DeleteQueue(
                    connection: sqlConnection,
                    schema: "dbo",
                    queueName: "audit");
                QueueDeletionUtils.DeleteQueue(
                    connection: sqlConnection,
                    schema: "dbo",
                    queueName: "error");
            }

        }
    }
}
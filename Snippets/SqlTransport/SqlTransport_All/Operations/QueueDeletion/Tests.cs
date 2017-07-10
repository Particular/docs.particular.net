using System.Data.SqlClient;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SqlServer_All.Operations.QueueDeletion
{
    [TestFixture]
    [Explicit]
    public class Tests
    {

        [Test]
        public async Task DeleteQueuesForEndpoint()
        {
            var connectionString = @"Data Source=.\SqlExpress;Database=Snippets.SqlTransport;Integrated Security=True";
            await SqlHelper.EnsureDatabaseExists(connectionString).ConfigureAwait(false);
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync()
                    .ConfigureAwait(false);
                await DeleteEndpointQueues.DeleteQueuesForEndpoint(
                        connection: sqlConnection,
                        schema: "dbo",
                        endpointName: "myendpoint")
                    .ConfigureAwait(false);
            }
        }

        [Test]
        public async Task DeleteSharedQueues()
        {
            var connectionString = @"Data Source=.\SqlExpress;Database=samples;Integrated Security=True";
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync()
                    .ConfigureAwait(false);
                await QueueDeletionUtils.DeleteQueue(
                        connection: sqlConnection,
                        schema: "dbo",
                        queueName: "audit")
                    .ConfigureAwait(false);
                await QueueDeletionUtils.DeleteQueue(
                        connection: sqlConnection,
                        schema: "dbo",
                        queueName: "error")
                    .ConfigureAwait(false);
            }

        }
    }
}
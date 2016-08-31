using System.Data.SqlClient;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SqlServer_All.Operations.QueueCreation
{
    [TestFixture]
    [Explicit]
    public class Tests
    {
        [Test]
        public async Task CreateQueuesForEndpoint()
        {
            var connectionString = @"Data Source=.\SqlExpress;Database=samples;Integrated Security=True";
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync()
                    .ConfigureAwait(false);
                await CreateEndpointQueues.CreateQueuesForEndpoint(
                        connection: sqlConnection,
                        schema: "dbo",
                        endpointName: "myendpoint")
                    .ConfigureAwait(false);

                await QueueCreationUtils.CreateQueue(
                        connection: sqlConnection,
                        schema: "dbo",
                        queueName: "error")
                    .ConfigureAwait(false);

                await QueueCreationUtils.CreateQueue(
                        connection: sqlConnection,
                        schema: "dbo",
                        queueName: "audit")
                    .ConfigureAwait(false);
            }

        }

    }
}
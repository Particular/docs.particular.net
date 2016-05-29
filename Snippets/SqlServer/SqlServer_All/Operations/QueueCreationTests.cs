using System.Data.SqlClient;
using System.Threading.Tasks;
using NUnit.Framework;

[TestFixture]
[Explicit]
public class QueueCreationTests
{
    [Test]
    public async Task CreateQueuesForEndpoint()
    {
        var connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=samples;Integrated Security=True";
        using (var sqlConnection = new SqlConnection(connectionString))
        {
            await sqlConnection.OpenAsync()
                .ConfigureAwait(false);
            await QueueCreation.CreateQueuesForEndpoint(
                connection: sqlConnection,
                schema: "dbo",
                endpointName: "myendpoint")
                .ConfigureAwait(false);

            await QueueCreation.CreateQueue(
                connection: sqlConnection,
                schema: "dbo",
                queueName: "error")
                .ConfigureAwait(false);

            await QueueCreation.CreateQueue(
                connection: sqlConnection,
                schema: "dbo",
                queueName: "audit")
                .ConfigureAwait(false);
        }

    }

}
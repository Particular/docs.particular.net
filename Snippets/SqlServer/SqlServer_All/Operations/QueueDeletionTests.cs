using System.Data.SqlClient;
using System.Threading.Tasks;
using NUnit.Framework;

[TestFixture]
[Explicit]
public class QueueDeletionTests
{

    [Test]
    public async Task DeleteQueuesForEndpoint()
    {
        var connectionString = @"Data Source=.\SqlExpress;Database=samples;Integrated Security=True";
        using (var sqlConnection = new SqlConnection(connectionString))
        {
            await sqlConnection.OpenAsync()
                .ConfigureAwait(false);
            await QueueDeletion.DeleteQueuesForEndpoint(
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
            await QueueDeletion.DeleteQueue(
                connection: sqlConnection,
                schema: "dbo",
                queueName: "audit")
                .ConfigureAwait(false);
            await QueueDeletion.DeleteQueue(
                connection: sqlConnection,
                schema: "dbo",
                queueName: "error")
                .ConfigureAwait(false);
        }

    }
}
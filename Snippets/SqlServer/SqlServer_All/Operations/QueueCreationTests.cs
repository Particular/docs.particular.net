using System.Data.SqlClient;
using NUnit.Framework;

[TestFixture]
[Explicit]
public class QueueCreationTests
{
    [Test]
    public void CreateQueuesForEndpoint()
    {
        var connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=samples;Integrated Security=True";
        using (var sqlConnection = new SqlConnection(connectionString))
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
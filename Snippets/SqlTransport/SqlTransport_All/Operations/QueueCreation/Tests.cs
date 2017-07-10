using System.Data.SqlClient;
using NUnit.Framework;
using System.IO;
using System.Management.Automation;

namespace SqlServer_All.Operations.QueueCreation
{

    [TestFixture]
    [Explicit]
    public class Tests
    {
        [Test]
        public void CreateQueuesForEndpoint()
        {
            var connectionString = @"Data Source=.\SqlExpress;Database=Snippets.SqlTransport;Integrated Security=True";
            SqlHelper.EnsureDatabaseExists(connectionString);
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                CreateEndpointQueues.CreateQueuesForEndpoint(
                        connection: sqlConnection,
                        schema: "dbo",
                        endpointName: "myendpoint");

                QueueCreationUtils.CreateQueue(
                        connection: sqlConnection,
                        schema: "dbo",
                        queueName: "error");

                QueueCreationUtils.CreateQueue(
                        connection: sqlConnection,
                        schema: "dbo",
                        queueName: "audit");
            }

        }

        [Test]
        public void CreateQueuesForEndpointPs()
        {
            var connectionString = @"Data Source=.\SqlExpress;Database=Snippets.SqlTransport;Integrated Security=True";
            SqlHelper.EnsureDatabaseExists(connectionString);

            var scriptPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Operations/QueueCreation/QueueCreation.ps1");
            using (var powerShell = PowerShell.Create())
            {
                powerShell.AddScript(File.ReadAllText(scriptPath));
                powerShell.Invoke();
                var command = powerShell.AddCommand("CreateQueuesForEndpoint");
                command.AddParameter("connection", connectionString);
                command.AddParameter("endpointName", "myendpoint");
                command.Invoke();
            }
        }

    }
}
using System.Data.SqlClient;
using System.Threading.Tasks;
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
        public async Task CreateQueuesForEndpoint()
        {
            var connectionString = @"Data Source=.\SqlExpress;Database=Snippets.SqlTransport;Integrated Security=True";
            await SqlHelper.EnsureDatabaseExists(connectionString).ConfigureAwait(false);
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

        [Test]
        public async Task CreateQueuesForEndpointPs()
        {
            var connectionString = @"Data Source=.\SqlExpress;Database=Snippets.SqlTransport;Integrated Security=True";
            await SqlHelper.EnsureDatabaseExists(connectionString).ConfigureAwait(false);

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
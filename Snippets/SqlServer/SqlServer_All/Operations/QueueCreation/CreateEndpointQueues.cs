using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SqlServer_All.Operations.QueueCreation
{
    public class CreateEndpointQueues
    {
        async Task CreateQueuesForEndpoint()
        {
            var connectionString = @"Data Source=.\SqlExpress;Database=samples;Integrated Security=True";

            #region sqlserver-create-queues-endpoint-usage

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync()
                    .ConfigureAwait(false);
                await CreateQueuesForEndpoint(
                        connection: sqlConnection,
                        schema: "dbo",
                        endpointName: "myendpoint")
                    .ConfigureAwait(false);
            }

            #endregion
        }

        #region sqlserver-create-queues-for-endpoint [,2]

        public static async Task CreateQueuesForEndpoint(SqlConnection connection, string schema, string endpointName)
        {
            // main queue
            await QueueCreationUtils.CreateQueue(connection, schema, endpointName)
                .ConfigureAwait(false);

            // callback queue
            await QueueCreationUtils.CreateQueue(connection, schema, $"{endpointName}.{Environment.MachineName}")
                .ConfigureAwait(false);

            // retries queue
            await QueueCreationUtils.CreateQueue(connection, schema, $"{endpointName}.Retries")
                .ConfigureAwait(false);

            // timeout queue
            await QueueCreationUtils.CreateQueue(connection, schema, $"{endpointName}.Timeouts")
                .ConfigureAwait(false);

            // timeout dispatcher queue
            await QueueCreationUtils.CreateQueue(connection, schema, $"{endpointName}.TimeoutsDispatcher")
                .ConfigureAwait(false);
        }

        #endregion
    }
}
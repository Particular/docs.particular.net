using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SqlServer_All.Operations.QueueCreation
{
    public static class CreateEndpointQueues
    {

        static async Task CreateQueuesForEndpoint()
        {
            var connectionString = @"Data Source=.\SqlExpress;Database=samples;Integrated Security=True";

            #region sqlserver-create-queues-endpoint-usage

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync()
                    .ConfigureAwait(false);
                await CreateQueuesForEndpoint(
                        connection: connection,
                        schema: "dbo",
                        endpointName: "myendpoint")
                    .ConfigureAwait(false);
            }

            #endregion
        }
        #region sqlserver-create-queues-for-endpoint

        public static async Task CreateQueuesForEndpoint(SqlConnection connection, string schema, string endpointName)
        {
            // main queue
            await QueueCreationUtils.CreateQueue(connection, schema, endpointName)
                .ConfigureAwait(false);

            // callback queue
            await QueueCreationUtils.CreateQueue(connection, schema, $"{endpointName}.{Environment.MachineName}")
                .ConfigureAwait(false);

            // timeout queue
            await QueueCreationUtils.CreateQueue(connection, schema, $"{endpointName}.Timeouts")
                .ConfigureAwait(false);

            // timeout dispatcher queue
            await QueueCreationUtils.CreateQueue(connection, schema, $"{endpointName}.TimeoutsDispatcher")
                .ConfigureAwait(false);

            // retries queue
            // TODO: Only required in Versions 2 and below
            await QueueCreationUtils.CreateQueue(connection, schema, $"{endpointName}.Retries")
                .ConfigureAwait(false);
        }

        #endregion
    }
}
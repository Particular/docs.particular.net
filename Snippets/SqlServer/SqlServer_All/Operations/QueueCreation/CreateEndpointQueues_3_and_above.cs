using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SqlServer_All.Operations.QueueCreation
{
    public static class CreateEndpointQueues_3_and_above
    {

        #region sqlserver-create-queues-for-endpoint [3,]

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
        }

        #endregion
    }
}
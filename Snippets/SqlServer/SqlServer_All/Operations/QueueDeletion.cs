using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

public static class QueueDeletion
{
    public static async Task DeleteQueuesForEndpoint()
    {
        var connectionString = @"Data Source=.\SqlExpress;Database=samples;Integrated Security=True";

        #region sqlserver-delete-queues-endpoint-usage

        using (var sqlConnection = new SqlConnection(connectionString))
        {
            await sqlConnection.OpenAsync()
                .ConfigureAwait(false);
            await DeleteQueuesForEndpoint(
                connection: sqlConnection,
                schema: "dbo",
                endpointName: "myendpoint")
                .ConfigureAwait(false);
        }

        #endregion
    }

    public static async Task DeleteSharedQueues()
    {
        var connectionString = @"Data Source=.\SqlExpress;Database=samples;Integrated Security=True";

        #region sqlserver-delete-queues-shared-usage

        using (var sqlConnection = new SqlConnection(connectionString))
        {
            await sqlConnection.OpenAsync()
                .ConfigureAwait(false);
            await DeleteQueue(
                connection: sqlConnection,
                schema: "dbo",
                queueName: "audit")
                .ConfigureAwait(false);
            await DeleteQueue(
                connection: sqlConnection,
                schema: "dbo",
                queueName: "error")
                .ConfigureAwait(false);
        }

        #endregion
    }

    #region sqlserver-delete-queues

    public static async Task DeleteQueue(SqlConnection connection, string schema, string queueName)
    {
        var deleteScript = $@"
                IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[{schema}].[{queueName}]') AND type in (N'U'))
                DROP TABLE [{schema}].[{queueName}]";
        using (var command = new SqlCommand(deleteScript, connection))
        {
            await command.ExecuteNonQueryAsync()
                .ConfigureAwait(false);
        }
    }

    public static async Task DeleteQueuesForEndpoint(SqlConnection connection, string schema, string endpointName)
    {
        // main queue
        await DeleteQueue(connection, schema, endpointName)
            .ConfigureAwait(false);

        // callback queue
        await DeleteQueue(connection, schema, $"{endpointName}.{Environment.MachineName}")
            .ConfigureAwait(false);

        // retries queue
        await DeleteQueue(connection, schema, $"{endpointName}.Retries")
            .ConfigureAwait(false);

        // timeout queue
        await DeleteQueue(connection, schema, $"{endpointName}.Timeouts")
            .ConfigureAwait(false);

        // timeout dispatcher queue
        await DeleteQueue(connection, schema, $"{endpointName}.TimeoutsDispatcher")
            .ConfigureAwait(false);
    }

    #endregion
}
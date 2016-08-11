using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

public static class QueueCreation
{
    public static async Task CreateQueuesForEndpoint()
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

        #region sqlserver-create-queues-shared-usage

        using (var sqlConnection = new SqlConnection(connectionString))
        {
            await sqlConnection.OpenAsync()
                .ConfigureAwait(false);
            await CreateQueue(
                connection: sqlConnection,
                schema: "dbo",
                queueName: "error")
                .ConfigureAwait(false);

            await CreateQueue(
                connection: sqlConnection,
                schema: "dbo",
                queueName: "audit")
                .ConfigureAwait(false);
        }

        #endregion
    }

    #region sqlserver-create-queues

    public static async Task CreateQueuesForEndpoint(SqlConnection connection, string schema, string endpointName)
    {
        // main queue
        await CreateQueue(connection, schema, endpointName)
            .ConfigureAwait(false);

        // callback queue
        await CreateQueue(connection, schema, $"{endpointName}.{Environment.MachineName}")
            .ConfigureAwait(false);

        // retries queue
        await CreateQueue(connection, schema, $"{endpointName}.Retries")
            .ConfigureAwait(false);

        // timeout queue
        await CreateQueue(connection, schema, $"{endpointName}.Timeouts")
            .ConfigureAwait(false);

        // timeout dispatcher queue
        await CreateQueue(connection, schema, $"{endpointName}.TimeoutsDispatcher")
            .ConfigureAwait(false);
    }

    public static async Task CreateQueue(SqlConnection connection, string schema, string queueName)
    {
        var sql = $@"IF NOT  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[{schema}].[{queueName}]') AND type in (N'U'))
                BEGIN
                CREATE TABLE [{schema}].[{queueName}](
	                [Id] [uniqueidentifier] NOT NULL,
	                [CorrelationId] [varchar](255),
	                [ReplyToAddress] [varchar](255),
	                [Recoverable] [bit] NOT NULL,
	                [Expires] [datetime],
	                [Headers] [varchar](max) NOT NULL,
	                [Body] [varbinary](max),
	                [RowVersion] [bigint] IDENTITY(1,1) NOT NULL
                ) ON [PRIMARY];
                CREATE CLUSTERED INDEX [Index_RowVersion] ON [{schema}].[{queueName}]
                (
	                [RowVersion] ASC
                ) ON [PRIMARY]
                END";
        using (var command = new SqlCommand(sql, connection))
        {
            await command.ExecuteNonQueryAsync()
                .ConfigureAwait(false);
        }
    }

#endregion
}
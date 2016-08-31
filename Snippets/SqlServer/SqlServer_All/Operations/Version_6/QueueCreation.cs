namespace SqlServer_All.Operations.Version_6
{
    using System;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    public static class QueueCreation
    {
        #region sqlserver-create-queues

        public static async Task CreateQueuesForEndpoint(SqlConnection connection, string schema, string endpointName)
        {
            // main queue
            await CreateQueue(connection, schema, endpointName)
                .ConfigureAwait(false);

            // callback queue
            await CreateQueue(connection, schema, $"{endpointName}.{Environment.MachineName}")
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
}
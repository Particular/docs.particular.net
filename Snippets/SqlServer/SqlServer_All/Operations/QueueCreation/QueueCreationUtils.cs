using System.Data.SqlClient;
using System.Threading.Tasks;

#region sqlserver-create-queues

namespace SqlServer_All.Operations.QueueCreation
{
    public static class QueueCreationUtils
    {
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
        CREATE NONCLUSTERED INDEX [Index_Expires] ON [{schema}].[{queueName}]
            (
            [Expires] ASC
            )
            INCLUDE
            (
            [Id],
            [RowVersion]
        )
                END";
            using (var command = new SqlCommand(sql, connection))
            {
                await command.ExecuteNonQueryAsync()
                    .ConfigureAwait(false);
            }
        }

    }
}
#endregion
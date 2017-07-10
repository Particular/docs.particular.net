using System.Data.SqlClient;
using System.Threading.Tasks;

#region sqlserver-create-queues

namespace SqlServer_All.Operations.QueueCreation
{
    public static class QueueCreationUtils
    {
        public static async Task CreateQueue(SqlConnection connection, string schema, string queueName)
        {
            var sql = $@"
            if not  exists (select * from sys.objects where object_id = object_id(N'[{schema}].[{queueName}]') and type in (N'U'))
                begin
                create table [{schema}].[{queueName}](
                    [Id] [uniqueidentifier] not null,
                    [CorrelationId] [varchar](255),
                    [ReplyToAddress] [varchar](255),
                    [Recoverable] [bit] not null,
                    [Expires] [datetime],
                    [Headers] [varchar](max) not null,
                    [Body] [varbinary](max),
                    [RowVersion] [bigint] identity(1,1) not null
                ) on [primary];
                create clustered index [Index_RowVersion] on [{schema}].[{queueName}]
                (
                    [RowVersion] asc
                ) on [primary]
            create nonclustered index [Index_Expires] on [{schema}].[{queueName}]
            (
                [Expires] asc
            )
            include
            (
                [Id],
                [RowVersion]
            )
            end";
            using (var command = new SqlCommand(sql, connection))
            {
                await command.ExecuteNonQueryAsync()
                    .ConfigureAwait(false);
            }
        }

    }
}
#endregion
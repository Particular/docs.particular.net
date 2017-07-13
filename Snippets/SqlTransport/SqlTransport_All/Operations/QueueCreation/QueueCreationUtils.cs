using System.Data.SqlClient;

namespace SqlServer_All.Operations.QueueCreation
{

    #region create-queues
    public static class QueueCreationUtils
    {
        public static void CreateQueue(SqlConnection connection, string schema, string queueName)
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
                    [Headers] [nvarchar](max) not null,
                    [Body] [varbinary](max),
                    [RowVersion] [bigint] identity(1,1) not null
                );
                create clustered index [Index_RowVersion] on [{schema}].[{queueName}]
                (
                    [RowVersion]
                )
                create nonclustered index [Index_Expires] on [{schema}].[{queueName}]
                (
                    [Expires]
                )
                include
                (
                    [Id],
                    [RowVersion]
                )
                where [Expires] is not null
            end";
            using (var command = new SqlCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        public static void CreateDelayedQueue(SqlConnection connection, string schema, string queueName)
        {
            var sql = $@"
            if not  exists (select * from sys.objects where object_id = object_id(N'[{schema}].[{queueName}]') and type in (N'U'))
            begin
                create table [{schema}].[{queueName}](
                    [Headers] nvarchar(max) not null,
                    [Body] varbinary(max),
                    [Due] datetime not null,
                    [RowVersion] bigint identity(1,1) not null
                );

                create nonclustered index [Index_Due] on [{schema}].[{queueName}]
                (
                    [Due]
                )
            end";
            using (var command = new SqlCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }

    }
    #endregion
}

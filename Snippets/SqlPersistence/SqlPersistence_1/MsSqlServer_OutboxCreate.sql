startcode MsSqlServer_OutboxCreateSql
declare @tableName nvarchar(max) = @tablePrefix + 'OutboxData';

if not exists (
    select * from sys.objects
    where
        object_id = object_id(@tableName)
        and type in (N'U')
)
begin
declare @createTable nvarchar(max);
set @createTable = N'
    create table ' + @tableName + '(
        MessageId nvarchar(1024) not null primary key,
        Dispatched bit not null default 0,
        DispatchedAt datetime,
        PersistenceVersion varchar(23) not null,
        Operations nvarchar(max) not null
    )
';
exec(@createTable);
end

endcode

startcode MsSqlServer_OutboxCreateSql
declare @tableName nvarchar(max) = '[' + @schema + '].[' + @tablePrefix + 'OutboxData]';

if not exists (
    select * from sys.objects
    where
        object_id = object_id(@tableName)
        and type in ('U')
)
begin
declare @createTable nvarchar(max);
set @createTable = '
    create table ' + @tableName + '(
        MessageId nvarchar(200) not null primary key nonclustered,
        Dispatched bit not null default 0,
        DispatchedAt datetime,
        PersistenceVersion varchar(23) not null,
        Operations nvarchar(max) not null
    )
';
exec(@createTable);
end
endcode

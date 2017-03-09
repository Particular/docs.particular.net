startcode MsSqlServer_TimeoutCreateSql
declare @tableName nvarchar(max) = '[' + @schema + '].[' + @tablePrefix + 'TimeoutData]';

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
        Id uniqueidentifier not null primary key,
        Destination nvarchar(200),
        SagaId uniqueidentifier,
        State varbinary(max),
        Time datetime,
        Headers nvarchar(max) not null,
        PersistenceVersion varchar(23) not null
    )
';
exec(@createTable);
end
endcode

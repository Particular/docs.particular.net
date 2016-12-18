startcode MsSqlServer_TimeoutCreateSql
declare @tableName nvarchar(max) = @tablePrefix + 'TimeoutData';

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
        Id uniqueidentifier not null primary key,
        Destination nvarchar(1024),
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

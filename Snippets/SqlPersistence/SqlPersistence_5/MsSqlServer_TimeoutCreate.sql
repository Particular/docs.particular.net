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

if not exists
(
    select *
    from sys.indexes
    where
        name = 'Index_SagaId' and
        object_id = object_id(@tableName)
)
begin
  declare @createSagaIdIndex nvarchar(max);
  set @createSagaIdIndex = '
  create index Index_SagaId
  on ' + @tableName + '(SagaId);';
  exec(@createSagaIdIndex);
end

if not exists
(
    select *
    from sys.indexes
    where
        name = 'Index_Time' and
        object_id = object_id(@tableName)
)
begin
  declare @createTimeIndex nvarchar(max);
  set @createTimeIndex = '
  create index Index_Time
  on ' + @tableName + '(Time);';
  exec(@createTimeIndex);
end
endcode

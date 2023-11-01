startcode MsSqlServer_SagaCreateSql

/* TableNameVariable */

declare @tableName nvarchar(max) = '[' + @schema + '].[' + @tablePrefix + N'OrderSaga]';
declare @tableNameWithoutSchema nvarchar(max) = @tablePrefix + N'OrderSaga';


/* Initialize */

/* CreateTable */

if not exists
(
    select *
    from sys.objects
    where
        object_id = object_id(@tableName) and
        type in ('U')
)
begin
declare @createTable nvarchar(max);
set @createTable = '
    create table ' + @tableName + '(
        Id uniqueidentifier not null primary key,
        Metadata nvarchar(max) not null,
        Data nvarchar(max) not null,
        PersistenceVersion varchar(23) not null,
        SagaTypeVersion varchar(23) not null,
        Concurrency int not null
    )
';
exec(@createTable);
end

/* AddProperty OrderNumber */

if not exists
(
  select * from sys.columns
  where
    name = N'Correlation_OrderNumber' and
    object_id = object_id(@tableName)
)
begin
  declare @createColumn_OrderNumber nvarchar(max);
  set @createColumn_OrderNumber = '
  alter table ' + @tableName + N'
    add Correlation_OrderNumber bigint;';
  exec(@createColumn_OrderNumber);
end

/* VerifyColumnType Int */

declare @dataType_OrderNumber nvarchar(max);
set @dataType_OrderNumber = (
  select data_type
  from INFORMATION_SCHEMA.COLUMNS
  where
    table_name = @tableNameWithoutSchema and
    table_schema = @schema and
    column_name = 'Correlation_OrderNumber'
);
if (@dataType_OrderNumber <> 'bigint')
  begin
    declare @error_OrderNumber nvarchar(max) = N'Incorrect data type for Correlation_OrderNumber. Expected bigint got ' + @dataType_OrderNumber + '.';
    throw 50000, @error_OrderNumber, 0
  end

/* WriteCreateIndex OrderNumber */

if not exists
(
    select *
    from sys.indexes
    where
        name = N'Index_Correlation_OrderNumber' and
        object_id = object_id(@tableName)
)
begin
  declare @createIndex_OrderNumber nvarchar(max);
  set @createIndex_OrderNumber = N'
  create unique index Index_Correlation_OrderNumber
  on ' + @tableName + N'(Correlation_OrderNumber)
  where Correlation_OrderNumber is not null;';
  exec(@createIndex_OrderNumber);
end

/* AddProperty OrderId */

if not exists
(
  select * from sys.columns
  where
    name = N'Correlation_OrderId' and
    object_id = object_id(@tableName)
)
begin
  declare @createColumn_OrderId nvarchar(max);
  set @createColumn_OrderId = '
  alter table ' + @tableName + N'
    add Correlation_OrderId uniqueidentifier;';
  exec(@createColumn_OrderId);
end

/* VerifyColumnType Guid */

declare @dataType_OrderId nvarchar(max);
set @dataType_OrderId = (
  select data_type
  from INFORMATION_SCHEMA.COLUMNS
  where
    table_name = @tableNameWithoutSchema and
    table_schema = @schema and
    column_name = 'Correlation_OrderId'
);
if (@dataType_OrderId <> 'uniqueidentifier')
  begin
    declare @error_OrderId nvarchar(max) = N'Incorrect data type for Correlation_OrderId. Expected uniqueidentifier got ' + @dataType_OrderId + '.';
    throw 50000, @error_OrderId, 0
  end

/* CreateIndex OrderId */

if not exists
(
    select *
    from sys.indexes
    where
        name = N'Index_Correlation_OrderId' and
        object_id = object_id(@tableName)
)
begin
  declare @createIndex_OrderId nvarchar(max);
  set @createIndex_OrderId = N'
  create unique index Index_Correlation_OrderId
  on ' + @tableName + N'(Correlation_OrderId)
  where Correlation_OrderId is not null;';
  exec(@createIndex_OrderId);
end

/* PurgeObsoleteIndex */

declare @dropIndexQuery nvarchar(max);
select @dropIndexQuery =
(
    select 'drop index ' + name + ' on ' + @tableName + ';'
    from sysindexes
    where
        Id = object_id(@tableName) and
        Name is not null and
        Name like 'Index_Correlation_%' and
        Name <> N'Index_Correlation_OrderNumber' and
        Name <> N'Index_Correlation_OrderId'
);
exec sp_executesql @dropIndexQuery

/* PurgeObsoleteProperties */

declare @dropPropertiesQuery nvarchar(max);
select @dropPropertiesQuery =
(
    select 'alter table ' + @tableName + ' drop column ' + column_name + ';'
    from INFORMATION_SCHEMA.COLUMNS
    where
        table_name = @tableNameWithoutSchema and
        table_schema = @schema and
        column_name like 'Correlation_%' and
        column_name <> N'Correlation_OrderNumber' and
        column_name <> N'Correlation_OrderId'
);
exec sp_executesql @dropPropertiesQuery

/* CompleteSagaScript */

endcode

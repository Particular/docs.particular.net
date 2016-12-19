startcode MySql_SagaCreateSql

/* TableNameVariable */

set @tableName = concat(@tablePrefix, 'OrderSaga');


/* CreateTable */

set @createTable = concat('
    create table if not exists ', @tableName, '(
        Id varchar(38) not null,
        Metadata json not null,
        Data json not null,
        PersistenceVersion varchar(23) not null,
        SagaTypeVersion varchar(23) not null,
        SagaVersion int not null,
        primary key (Id)
    ) default charset=utf8;
');
prepare statment from @createTable;
execute statment;
deallocate prepare statment;

/* AddProperty OrderNumber */

select count(*)
into @exist
from information_schema.columns
where table_schema = database() and
      column_name = 'Correlation_OrderNumber' and
      table_name = @tableName;

set @query = IF(
    @exist <= 0,
    concat('alter table ', @tableName, ' add column Correlation_OrderNumber bigint'), 'select \'Column Exists\' status');

prepare statment from @query;
execute statment;
deallocate prepare statment;

/* VerifyColumnType Int */

set @column_type_OrderNumber = (
  select column_type
  from information_schema.columns
  where
    table_schema = database() and
    table_name = @tableName and
    column_name = 'Correlation_OrderNumber'
);

set @query = IF(
    @column_type_OrderNumber <> 'bigint',
    'SIGNAL SQLSTATE \'45000\' SET MESSAGE_TEXT = \'Incorrect data type for Correlation_OrderNumber\'',
    'select \'Column Type OK\' status');

prepare statment from @query;
execute statment;
deallocate prepare statment;

/* WriteCreateIndex OrderNumber */

select count(*)
into @exist
from information_schema.statistics
where
    table_schema = database() and
    index_name = 'Index_Correlation_OrderNumber' and
    table_name = @tableName;

set @query = IF(
    @exist <= 0,
    concat('create unique index Index_Correlation_OrderNumber on ', @tableName, '(Correlation_OrderNumber)'), 'select \'Index Exists\' status');

prepare statment from @query;
execute statment;
deallocate prepare statment;

/* AddProperty OrderId */

select count(*)
into @exist
from information_schema.columns
where table_schema = database() and
      column_name = 'Correlation_OrderId' and
      table_name = @tableName;

set @query = IF(
    @exist <= 0,
    concat('alter table ', @tableName, ' add column Correlation_OrderId varchar(38)'), 'select \'Column Exists\' status');

prepare statment from @query;
execute statment;
deallocate prepare statment;

/* VerifyColumnType Guid */

set @column_type_OrderId = (
  select column_type
  from information_schema.columns
  where
    table_schema = database() and
    table_name = @tableName and
    column_name = 'Correlation_OrderId'
);

set @query = IF(
    @column_type_OrderId <> 'varchar(38)',
    'SIGNAL SQLSTATE \'45000\' SET MESSAGE_TEXT = \'Incorrect data type for Correlation_OrderId\'',
    'select \'Column Type OK\' status');

prepare statment from @query;
execute statment;
deallocate prepare statment;

/* CreateIndex OrderId */

select count(*)
into @exist
from information_schema.statistics
where
    table_schema = database() and
    index_name = 'Index_Correlation_OrderId' and
    table_name = @tableName;

set @query = IF(
    @exist <= 0,
    concat('create unique index Index_Correlation_OrderId on ', @tableName, '(Correlation_OrderId)'), 'select \'Index Exists\' status');

prepare statment from @query;
execute statment;
deallocate prepare statment;

/* PurgeObsoleteIndex */

select concat('drop index ', index_name, ' on ', @tableName, ';')
from information_schema.statistics
where
    table_schema = database() and
    table_name = @tableName and
    index_name like 'Index_Correlation_%' and
    index_name <> 'Index_Correlation_OrderNumber' and
    index_name <> 'Index_Correlation_OrderId' and
    table_schema = database()
into @dropIndexQuery;
select if (
    @dropIndexQuery is not null,
    @dropIndexQuery,
    'select ''no index to delete'';')
    into @dropIndexQuery;

prepare statment from @dropIndexQuery;
execute statment;
deallocate prepare statment;

/* PurgeObsoleteProperties */

select concat('alter table ', @tableName, ' drop column ', column_name, ';')
from information_schema.columns
where
    table_schema = database() and
    table_name = @tableName and
    column_name like 'Correlation_%' and
    column_name <> 'Correlation_OrderNumber' and
    column_name <> 'Correlation_OrderId'
into @dropPropertiesQuery;

select if (
    @dropPropertiesQuery is not null,
    @dropPropertiesQuery,
    'select ''no property to delete'';')
    into @dropPropertiesQuery;

prepare statment from @dropPropertiesQuery;
execute statment;
deallocate prepare statment;

endcode

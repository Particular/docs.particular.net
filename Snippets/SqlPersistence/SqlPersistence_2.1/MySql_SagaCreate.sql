startcode MySql_SagaCreateSql

/* TableNameVariable */

set @tableNameQuoted = concat('`', @tablePrefix, 'OrderSaga`');
set @tableNameNonQuoted = concat(@tablePrefix, 'OrderSaga');


/* Initialize */

drop procedure if exists sqlpersistence_raiseerror;
create procedure sqlpersistence_raiseerror(message varchar(256))
begin
signal sqlstate
    'ERROR'
set
    message_text = message,
    mysql_errno = '45000';
end;

/* CreateTable */

set @createTable = concat('
    create table if not exists ', @tableNameQuoted, '(
        Id varchar(38) not null,
        Metadata json not null,
        Data json not null,
        PersistenceVersion varchar(23) not null,
        SagaTypeVersion varchar(23) not null,
        Concurrency int not null,
        primary key (Id)
    ) default charset=ascii;
');
prepare script from @createTable;
execute script;
deallocate prepare script;

/* AddProperty OrderNumber */

select count(*)
into @exist
from information_schema.columns
where table_schema = database() and
      column_name = 'Correlation_OrderNumber' and
      table_name = @tableNameNonQuoted;

set @query = IF(
    @exist <= 0,
    concat('alter table ', @tableNameQuoted, ' add column Correlation_OrderNumber bigint(20)'), 'select \'Column Exists\' status');

prepare script from @query;
execute script;
deallocate prepare script;

/* VerifyColumnType Int */

set @column_type_OrderNumber = (
  select concat(column_type,' character set ', character_set_name)
  from information_schema.columns
  where
    table_schema = database() and
    table_name = @tableNameNonQuoted and
    column_name = 'Correlation_OrderNumber'
);

set @query = IF(
    @column_type_OrderNumber <> 'bigint(20)',
    'call sqlpersistence_raiseerror(concat(\'Incorrect data type for Correlation_OrderNumber. Expected bigint(20) got \', @column_type_OrderNumber, \'.\'));',
    'select \'Column Type OK\' status');

prepare script from @query;
execute script;
deallocate prepare script;

/* WriteCreateIndex OrderNumber */

select count(*)
into @exist
from information_schema.statistics
where
    table_schema = database() and
    index_name = 'Index_Correlation_OrderNumber' and
    table_name = @tableNameNonQuoted;

set @query = IF(
    @exist <= 0,
    concat('create unique index Index_Correlation_OrderNumber on ', @tableNameQuoted, '(Correlation_OrderNumber)'), 'select \'Index Exists\' status');

prepare script from @query;
execute script;
deallocate prepare script;

/* AddProperty OrderId */

select count(*)
into @exist
from information_schema.columns
where table_schema = database() and
      column_name = 'Correlation_OrderId' and
      table_name = @tableNameNonQuoted;

set @query = IF(
    @exist <= 0,
    concat('alter table ', @tableNameQuoted, ' add column Correlation_OrderId varchar(38) character set ascii'), 'select \'Column Exists\' status');

prepare script from @query;
execute script;
deallocate prepare script;

/* VerifyColumnType Guid */

set @column_type_OrderId = (
  select concat(column_type,' character set ', character_set_name)
  from information_schema.columns
  where
    table_schema = database() and
    table_name = @tableNameNonQuoted and
    column_name = 'Correlation_OrderId'
);

set @query = IF(
    @column_type_OrderId <> 'varchar(38) character set ascii',
    'call sqlpersistence_raiseerror(concat(\'Incorrect data type for Correlation_OrderId. Expected varchar(38) character set ascii got \', @column_type_OrderId, \'.\'));',
    'select \'Column Type OK\' status');

prepare script from @query;
execute script;
deallocate prepare script;

/* CreateIndex OrderId */

select count(*)
into @exist
from information_schema.statistics
where
    table_schema = database() and
    index_name = 'Index_Correlation_OrderId' and
    table_name = @tableNameNonQuoted;

set @query = IF(
    @exist <= 0,
    concat('create unique index Index_Correlation_OrderId on ', @tableNameQuoted, '(Correlation_OrderId)'), 'select \'Index Exists\' status');

prepare script from @query;
execute script;
deallocate prepare script;

/* PurgeObsoleteIndex */

select concat('drop index ', index_name, ' on ', @tableNameQuoted, ';')
from information_schema.statistics
where
    table_schema = database() and
    table_name = @tableNameNonQuoted and
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

prepare script from @dropIndexQuery;
execute script;
deallocate prepare script;

/* PurgeObsoleteProperties */

select concat('alter table ', table_name, ' drop column ', column_name, ';')
from information_schema.columns
where
    table_schema = database() and
    table_name = @tableNameNonQuoted and
    column_name like 'Correlation_%' and
    column_name <> 'Correlation_OrderNumber' and
    column_name <> 'Correlation_OrderId'
into @dropPropertiesQuery;

select if (
    @dropPropertiesQuery is not null,
    @dropPropertiesQuery,
    'select ''no property to delete'';')
    into @dropPropertiesQuery;

prepare script from @dropPropertiesQuery;
execute script;
deallocate prepare script;

/* CompleteSagaScript */

endcode

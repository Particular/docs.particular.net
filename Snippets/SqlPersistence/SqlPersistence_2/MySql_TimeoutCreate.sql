startcode MySql_TimeoutCreateSql
set @tableName = concat('`', @tablePrefix, 'TimeoutData`');
set @createTable = concat('
    create table if not exists ', @tableName, '(
        Id varchar(38) not null,
        Destination nvarchar(200),
        SagaId varchar(38),
        State longblob,
        Time datetime,
        Headers json not null,
        PersistenceVersion varchar(23) not null,
        primary key (Id)
    ) default charset=ascii;
');
prepare script from @createTable;
execute script;
deallocate prepare script;


select count(*)
into @exist
from information_schema.statistics
where
    table_schema = database() and
    index_name = 'Index_SagaId' and
    table_name = @tableName;

set @query = IF(
    @exist <= 0,
    concat('create index Index_SagaId on ', @tableName, '(SagaId)'), 'select \'Index Exists\' status');

prepare script from @query;
execute script;
deallocate prepare script;


select count(*)
into @exist
from information_schema.statistics
where
    table_schema = database() and
    index_name = 'Index_Time' and
    table_name = @tableName;

set @query = IF(
    @exist <= 0,
    concat('create index Index_Time on ', @tableName, '(Time)'), 'select \'Index Exists\' status');

prepare script from @query;
execute script;
deallocate prepare script;
endcode

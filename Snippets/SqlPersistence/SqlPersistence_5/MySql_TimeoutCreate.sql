startcode MySql_TimeoutCreateSql
set @tableNameQuoted = concat('`', @tablePrefix, 'TimeoutData`');
set @tableNameNonQuoted = concat(@tablePrefix, 'TimeoutData');

set @createTable = concat('
    create table if not exists ', @tableNameQuoted, '(
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
    table_name = @tableNameNonQuoted;

set @query = IF(
    @exist <= 0,
    concat('create index Index_SagaId on ', @tableNameQuoted, '(SagaId)'), 'select \'Index Exists\' status');

prepare script from @query;
execute script;
deallocate prepare script;


select count(*)
into @exist
from information_schema.statistics
where
    table_schema = database() and
    index_name = 'Index_Time' and
    table_name = @tableNameNonQuoted;

set @query = IF(
    @exist <= 0,
    concat('create index Index_Time on ', @tableNameQuoted, '(Time)'), 'select \'Index Exists\' status');

prepare script from @query;
execute script;
deallocate prepare script;
endcode

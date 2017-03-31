startcode MySql_OutboxCreateSql
set @tableName = concat('`', @tablePrefix, 'OutboxData`');
set @createTable =  concat('
    create table if not exists ', @tableName, '(
        MessageId nvarchar(200) not null,
        Dispatched bit not null default 0,
        DispatchedAt datetime,
        PersistenceVersion varchar(23) not null,
        Operations json not null,
        primary key (MessageId)
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
    index_name = 'Index_DispatchedAt' and
    table_name = @tableName;

set @query = IF(
    @exist <= 0,
    concat('create index Index_DispatchedAt on ', @tableName, '(DispatchedAt)'), 'select \'Index Exists\' status');

prepare script from @query;
execute script;
deallocate prepare script;


select count(*)
into @exist
from information_schema.statistics
where
    table_schema = database() and
    index_name = 'Index_Dispatched' and
    table_name = @tableName;

set @query = IF(
    @exist <= 0,
    concat('create index Index_Dispatched on ', @tableName, '(Dispatched)'), 'select \'Index Exists\' status');

prepare script from @query;
execute script;
deallocate prepare script;
endcode

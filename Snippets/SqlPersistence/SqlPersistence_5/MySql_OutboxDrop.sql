startcode MySql_OutboxDropSql
set @tableName = concat('`', @tablePrefix, 'OutboxData`');

set @dropTable = concat('drop table if exists ', @tableName);
prepare script from @dropTable;
execute script;
deallocate prepare script;
endcode

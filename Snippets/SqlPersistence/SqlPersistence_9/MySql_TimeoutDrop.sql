startcode MySql_TimeoutDropSql
set @tableName = concat('`', @tablePrefix, 'TimeoutData`');

set @dropTable = concat('drop table if exists ', @tableName, '');
prepare script from @dropTable;
execute script;
deallocate prepare script;
endcode

startcode MySql_SubscriptionDropSql
set @tableName = concat('`', @tablePrefix, 'SubscriptionData`');

set @dropTable = concat('drop table if exists ', @tableName);
prepare script from @dropTable;
execute script;
deallocate prepare script;
endcode

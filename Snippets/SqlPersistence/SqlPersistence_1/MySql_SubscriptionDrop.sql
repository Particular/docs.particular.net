startcode MySql_SubscriptionDropSql
set @tableName = concat(@tablePrefix, 'SubscriptionData');
set @dropTable = concat('drop table if exists ', @tableName);
prepare statment from @dropTable;
execute statment;
deallocate prepare statment;
endcode

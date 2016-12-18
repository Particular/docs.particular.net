startcode MySql_TimeoutDropSql
set @tableName = concat(@tablePrefix, 'TimeoutData');
set @dropTable = concat('drop table if exists ', @tableName, '');
prepare statment from @dropTable;
execute statment;
deallocate prepare statment;
endcode

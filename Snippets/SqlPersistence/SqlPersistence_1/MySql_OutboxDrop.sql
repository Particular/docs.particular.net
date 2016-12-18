startcode MySql_OutboxDropSql
set @tableName = concat(@tablePrefix, 'OutboxData');
set @dropTable = concat('drop table if exists ', @tableName);
prepare statment from @dropTable;
execute statment;	   
deallocate prepare statment;
endcode

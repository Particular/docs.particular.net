startcode Oracle_SagaDropSql

/* TableNameVariable */

/* DropTable */

declare
  n number(10);
begin
  select count(*) into n from user_tables where table_name = 'ORDERSAGA';
  if(n > 0)
  then
    execute immediate 'drop table "ORDERSAGA"';
  end if;
end;

endcode

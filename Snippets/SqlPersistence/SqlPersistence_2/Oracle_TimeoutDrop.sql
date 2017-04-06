startcode Oracle_TimeoutDropSql
declare 
  tableName varchar2(30) := UPPER(:1) || 'TO';
  dropTable varchar2(50);
  n number(10);
begin
  select count(*) into n from user_tables where table_name = tableName;
  if(n = 1)
  then
    
    dropTable := 'DROP TABLE ' || tableName;
    
    execute immediate dropTable;
    
  end if;
end;
endcode

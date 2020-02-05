startcode Oracle_SubscriptionCreateSql
declare
  tableName varchar2(30) := UPPER(:1) || 'SS';
  createTable varchar2(500);
  n number(10);
begin
  select count(*) into n from user_tables where table_name = tableName;
  if(n = 0)
  then

    createTable :=
       'create table "' || tableName || '"
        (
          messagetype nvarchar2(200) not null,
          subscriber nvarchar2(200) not null,
          endpoint varchar2(200),
          persistenceversion varchar2(23),
          constraint "' || tableName || '_PK" primary key
          (
            messagetype
          , subscriber
          )
          enable
        )
        organization index';

    execute immediate createTable;

  end if;
end;
endcode

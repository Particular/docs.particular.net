startcode Oracle_OutboxCreateSql
declare
  tableName varchar2(30) := UPPER(:1) || 'OD';
  pkName varchar2(30) := tableName || '_PK';
  indexName varchar2(30) := tableName || '_IX';
  createTable varchar2(500);
  createIndex varchar2(500);
  n number(10);
begin
  select count(*) into n from user_tables where table_name = tableName;
  if(n = 0)
  then

    createTable :=
       'create table "' || tableName || '"
        (
          messageid nvarchar2(200) not null,
          dispatched number(1,0) default 0 not null check
          (
            dispatched in (0,1)
          ),
          dispatchedat timestamp,
          operations clob not null,
          persistenceversion varchar2(23) not null,
          constraint "' || pkName || '" primary key
          (
            messageid
          )
          enable
        )';

    execute immediate createTable;

  end if;

  select count(*) into n from user_indexes where index_name = indexName;
  if(n = 0)
  then

    createIndex :=
      'create index "' || indexName || '" on "' || tableName || '" (dispatched, dispatchedat)';

    execute immediate createIndex;

  end if;
end;
endcode

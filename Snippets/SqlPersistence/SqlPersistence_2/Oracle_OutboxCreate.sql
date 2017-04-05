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
       'CREATE TABLE ' || tableName || ' 
        (
          MESSAGEID NVARCHAR2(200) NOT NULL
        , DISPATCHED NUMBER(1,0) DEFAULT 0 NOT NULL CHECK
          (
            DISPATCHED IN (0,1)
          )
        , DISPATCHEDAT TIMESTAMP
        , OPERATIONS CLOB NOT NULL
        , PERSISTENCEVERSION VARCHAR2(23) NOT NULL
        , CONSTRAINT ' || pkName || ' PRIMARY KEY
          (
            MESSAGEID
          )
          ENABLE
        )';
    
    execute immediate createTable;
    
  end if;

  select count(*) into n from user_indexes where index_name = indexName;
  if(n = 0)
  then

    createIndex :=
      'CREATE INDEX ' || indexName || ' ON ' || tableName || ' (DISPATCHED, DISPATCHEDAT)';

    execute immediate createIndex;

  end if;
end;
endcode

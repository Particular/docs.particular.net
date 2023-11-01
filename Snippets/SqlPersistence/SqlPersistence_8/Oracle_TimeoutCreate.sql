startcode Oracle_TimeoutCreateSql
declare
  tableName varchar2(30) := UPPER(:1) || 'TO';
  timeIndex varchar2(30) := tableName || '_TK';
  sagaIndex varchar2(30) := tableName || '_SK';
  sqlStatement varchar2(500);
  n number(10);
begin
  select count(*) into n from user_tables where table_name = tableName;
  if(n = 0)
  then

    execute immediate
       'create table "' || tableName || '"
       (
         id varchar2(38) not null,
         destination nvarchar2(200) not null,
         sagaid varchar2(38),
         state blob,
         expiretime date,
         headers clob not null,
         persistenceversion varchar2(23) not null,
         constraint "' || tableName || '_PK" primary key
         (
           id
         )
         enable
       )';

  end if;

  select count(*) into n from user_indexes where index_name = timeIndex;
  if(n = 0)
  then
    execute immediate 'create index "' || timeIndex || '" on "' || tableName || '" (expiretime asc)';
  end if;

  select count(*) into n from user_indexes where index_name = sagaIndex;
  if(n = 0)
  then
    execute immediate 'create index "' || sagaIndex || '" on "' || tableName || '" (sagaid asc)';
  end if;
end;
endcode

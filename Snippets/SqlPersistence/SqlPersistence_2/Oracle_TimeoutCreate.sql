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
       'CREATE TABLE ' || tableName || ' 
		(
		  ID VARCHAR2(38) NOT NULL 
		, DESTINATION NVARCHAR2(200) NOT NULL 
		, SAGAID VARCHAR2(38) 
		, STATE BLOB 
		, EXPIRETIME DATE 
		, HEADERS CLOB NOT NULL 
		, PERSISTENCEVERSION VARCHAR2(23) NOT NULL 
		, CONSTRAINT ' || tableName || '_PK PRIMARY KEY 
		  (
			ID 
		  )
		  ENABLE 
		)';
    
  end if;

  select count(*) into n from user_indexes where index_name = timeIndex;
  if(n = 0)
  then
	execute immediate 'create index ' || timeIndex || ' on ' || tableName || ' (EXPIRETIME ASC)';
  end if;

  select count(*) into n from user_indexes where index_name = sagaIndex;
  if(n = 0)
  then
	execute immediate 'create index ' || sagaIndex || ' on ' || tableName || ' (SAGAID ASC)';
  end if;
end;
endcode

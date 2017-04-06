startcode Oracle_SagaCreateSql

/* TableNameVariable */

/* Initialize */

declare 
  sqlStatement varchar2(500);
  dataType varchar2(30);
  n number(10);
begin

/* CreateTable */

  select count(*) into n from user_tables where table_name = 'ORDERSAGA';
  if(n = 0)
  then
    
    sqlStatement :=
       'CREATE TABLE ORDERSAGA 
		(
          ID VARCHAR2(38) NOT NULL 
        , METADATA CLOB NOT NULL 
        , DATA CLOB NOT NULL 
        , PERSISTENCEVERSION VARCHAR2(23) NOT NULL 
        , SAGATYPEVERSION VARCHAR2(23) NOT NULL 
        , CONCURRENCY NUMBER(9) NOT NULL 
        , CONSTRAINT ORDERSAGA_PK PRIMARY KEY 
          (
            ID 
          )
          ENABLE 
        )';
    
    execute immediate sqlStatement;
    
  end if;

/* AddProperty OrderNumber */

select count(*) into n from ALL_TAB_COLUMNS where TABLE_NAME = 'ORDERSAGA' and COLUMN_NAME = 'CORR_ORDERNUMBER';
if(n = 0)
then
  sqlStatement := 'alter table ORDERSAGA add ( CORR_ORDERNUMBER NUMBER(19) )';
  
  execute immediate sqlStatement;
end if;

/* VerifyColumnType Int */

select DATA_TYPE ||
  case when CHAR_LENGTH > 0 then 
    '(' || CHAR_LENGTH || ')' 
  else
    case when DATA_PRECISION is not null then
      '(' || DATA_PRECISION ||
        case when DATA_SCALE is not null and DATA_SCALE > 0 then
          ',' || DATA_SCALE
        end || ')'
    end
  end into dataType
from ALL_TAB_COLUMNS 
where TABLE_NAME = 'ORDERSAGA' and COLUMN_NAME = 'CORR_ORDERNUMBER';

if(dataType <> 'NUMBER(19)')
then
  raise_application_error(-20000, 'Incorrect Correlation Property data type');
end if;

/* WriteCreateIndex OrderNumber */

select count(*) into n from user_indexes where table_name = 'ORDERSAGA' and index_name = 'SAGAIDX_599F57BA89CF9D164E3CFF';
if(n = 0)
then
  sqlStatement := 'create unique index SAGAIDX_599F57BA89CF9D164E3CFF on ORDERSAGA (CORR_ORDERNUMBER ASC)';

  execute immediate sqlStatement;
end if;

/* AddProperty OrderId */

select count(*) into n from ALL_TAB_COLUMNS where TABLE_NAME = 'ORDERSAGA' and COLUMN_NAME = 'CORR_ORDERID';
if(n = 0)
then
  sqlStatement := 'alter table ORDERSAGA add ( CORR_ORDERID VARCHAR2(38) )';
  
  execute immediate sqlStatement;
end if;

/* VerifyColumnType Guid */

select DATA_TYPE ||
  case when CHAR_LENGTH > 0 then 
    '(' || CHAR_LENGTH || ')' 
  else
    case when DATA_PRECISION is not null then
      '(' || DATA_PRECISION ||
        case when DATA_SCALE is not null and DATA_SCALE > 0 then
          ',' || DATA_SCALE
        end || ')'
    end
  end into dataType
from ALL_TAB_COLUMNS 
where TABLE_NAME = 'ORDERSAGA' and COLUMN_NAME = 'CORR_ORDERID';

if(dataType <> 'VARCHAR2(38)')
then
  raise_application_error(-20000, 'Incorrect Correlation Property data type');
end if;

/* CreateIndex OrderId */

select count(*) into n from user_indexes where table_name = 'ORDERSAGA' and index_name = 'SAGAIDX_FD8BAD844CFBBE419E43FE';
if(n = 0)
then
  sqlStatement := 'create unique index SAGAIDX_FD8BAD844CFBBE419E43FE on ORDERSAGA (CORR_ORDERID ASC)';

  execute immediate sqlStatement;
end if;

/* PurgeObsoleteIndex */

/* PurgeObsoleteProperties */

select count(*) into n
from ALL_TAB_COLUMNS
where TABLE_NAME = 'ORDERSAGA' and COLUMN_NAME LIKE 'CORR_%' and
        column_name <> 'CORR_ORDERNUMBER' and
        column_name <> 'CORR_ORDERID';
  
if(n > 0)
then

  select 'alter table ORDERSAGA drop column ' || COLUMN_NAME into sqlStatement
  from ALL_TAB_COLUMNS
  where TABLE_NAME = 'ORDERSAGA' and COLUMN_NAME LIKE 'CORR_%' and
        column_name <> 'CORR_ORDERNUMBER' and
        column_name <> 'CORR_ORDERID';
    
  execute immediate sqlStatement;

end if;

/* CreateComplete */
end;

endcode

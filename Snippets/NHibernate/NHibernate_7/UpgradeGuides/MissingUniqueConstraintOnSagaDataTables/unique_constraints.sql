

startcode DetectSagaDataDuplicates_MsSqlServer
declare @sagaDataTableName nvarchar(max) = '...'
declare @correlationPropertyColumnName nvarchar(max) = '...'
declare @sql nvarchar(max)

select @sql = 'select ' + @correlationPropertyColumnName + ', count(*) as SagaRows from ' + @sagaDataTableName + ' group by ' + @correlationPropertyColumnName + ' having count(*) > 1'
exec sp_executeSQL @sql
endcode


startcode AddUniqueConstraintOnSagaDataTable_MsSqlServer
declare @sagaDataTableName nvarchar(max) = '...'
declare @correlationPropertyColumnName nvarchar(max) = '...'
declare @sql nvarchar(max)

select @sql = 'alter table ' + @sagaDataTableName + ' add unique nonclustered ( ' + @correlationPropertyColumnName + ' asc )with (pad_index = off, statistics_norecompute = off, sort_in_tempdb = off, ignore_dup_key = off, online = off, allow_row_locks = on, allow_page_locks = on)'
exec sp_executeSQL @sql
endcode

startcode DetectSagaDataDuplicates_Oracle
declare
  sagaDataTableName varchar2(30) := '...';
  correlationPropertyColumnName varchar2(30) := '...';
  detectDuplicates varchar2(500);
  type SagaCursorType is ref cursor;
  sagaCursor SagaCursorType;
  correlationProperty <type of correlationPropertyColumnName>;
  instanceCount number(10);
begin
    detectDuplicates :=
       'select ' || correlationPropertyColumnName || ', count(*)
        from ' || sagaDataTableName || '
        group by ' || correlationPropertyColumnName || '
        having count(*) > 1';
        
    open sagaCursor for detectDuplicates;
    
    loop
      fetch sagaCursor into correlationProperty, instanceCount;
      exit when sagaCursor%NOTFOUND;
      
      dbms_output.put_line(correlationProperty);
      
    end loop;
    
    close sagaCursor;
end;
endcode


startcode AddUniqueConstraintOnSagaDataTable_Oracle
declare
  sagaDataTableName varchar2(30) := '...';
  correlationPropertyColumnName varchar2(30) := '...';
  uniqueConstraintName varchar2(30) := '...';
  addUniqueConstraint varchar2(500);
begin
    addUniqueConstraint :=
       'alter table ' || sagaDataTableName || ' add constraint ' || uniqueConstraintName || ' unique (' || correlationPropertyColumnName || ')';
       
    execute immediate addUniqueConstraint;
end;
endcode

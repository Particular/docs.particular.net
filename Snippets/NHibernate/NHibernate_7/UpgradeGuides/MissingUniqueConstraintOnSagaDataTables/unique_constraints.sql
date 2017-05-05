

startcode DetectSagaDataDuplicates_MsSqlServer
declare @sagaDataTableName nvarchar(max) = '...'
declare @correlationPropertyColumnName nvarchar(max) = '...'
declare @sql nvarchar(max)

select @sql = 'select ' + @correlationPropertyColumnName + ', count(*) as SagaRows from ' + @sagaDataTableName + ' group by ' + @correlationPropertyColumnName + ' having SagaRows > 1'
exec sp_executeSQL @sql
endcode


startcode AddUniqueConstraintOnSagaDataTable_MsSqlServer
declare @sagaDataTableName nvarchar(max) = '...'
declare @correlationPropertyColumnName nvarchar(max) = '...'
declare @sql nvarchar(max)


select @sql = 'alter table ' + @sagaDataTableName + ' add unique nonclustered ( ' + @correlationPropertyColumnName + ' asc )with (pad_index = off, statistics_norecompute = off, sort_in_tempdb = off, ignore_dup_key = off, online = off, allow_row_locks = on, allow_page_locks = on)'
exec sp_executeSQL @sql
endcode
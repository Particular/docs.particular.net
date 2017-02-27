startcode ConvertOutboxToNonClustered
declare @table nvarchar(max) = @tablePrefix + 'OutboxData';
declare @index nvarchar(max)
declare @dropSql nvarchar(max)
declare @createSql nvarchar(max)

select @index = si.name
from sys.tables st
  join sys.indexes si on st.object_id = si.object_id
where st.name = @table
  and si.is_primary_key = 1

select @dropSql = 'alter table ' + @table + ' drop constraint ' + @index
exec sp_executeSQL @dropSql;

select @createSql = 'alter table ' + @table + ' add constraint ' + @index + ' primary key nonclustered (MessageId)'
exec sp_executeSQL @createSql;

endcode

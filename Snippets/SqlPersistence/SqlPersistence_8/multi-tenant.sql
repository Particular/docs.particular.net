-- startcode MultiTenantOutboxCleanup

declare @BatchSize int = 5000
declare @DispatchedBefore datetime = dateadd(day, -7, getutcdate())

while 1=1
begin

	set rowcount @BatchSize
	delete from ENDPOINTNAME_OutboxData
	where Dispatched = 'true' and
		  DispatchedAt < @DispatchedBefore

	if @@ROWCOUNT < @BatchSize
		break;
end

-- endcode

-- startcode MultiTenantMultiEndpointOutboxCleanup
declare @SchemaAndName varchar(256)
declare @sql nvarchar(max)
declare @BatchSize int = 5000

declare OutboxTableCursor cursor for
select '[' + schema_name(schema_id) + '].[' + name + ']' as SchemaAndName
from sys.tables
where name like '%_OutboxData'

open OutboxTableCursor

fetch next from OutboxTableCursor into @SchemaAndName

while @@FETCH_STATUS = 0
begin

	print 'Cleaning ' + @SchemaAndName

	set @sql = N'

		declare @DispatchedBefore datetime = dateadd(day, -7, getutcdate())

		while 1=1
		begin

			delete top(' + cast(@BatchSize as varchar(50)) + ') from ' + @SchemaAndName + '
			where Dispatched = ''true'' and
				  DispatchedAt < @DispatchedBefore

			if @@ROWCOUNT < ' + cast(@BatchSize as varchar(50)) + '	
				break;
		end'

	execute sp_executesql @sql

	fetch next from OutboxTableCursor into @SchemaAndName
end

close OutboxTableCursor
deallocate OutboxTableCursor
-- endcode
-- startcode MigrateFromSql

declare @endpointName nvarchar(max) = N'PUT ENDPOINT NAME HERE';

declare @endpointSchema nvarchar(max) = N'dbo';
declare @dalayedTableSuffix nvarchar(max) = N'Delayed';
declare @timeoutDataTable nvarchar(max) = REPLACE(@endpointName,'.','_') + '_TimeoutData';

declare @migrateScript nvarchar(max);
set @migrateScript = N'
    with message as (
		select *
		from ' + @timeoutDataTable + ' with (updlock, readpast, rowlock)
		where [Time] is not null)
	delete from message
	output
		left(deleted.Headers, LEN(deleted.Headers) - 1) + '', "NServiceBus.SqlServer.ForwardDestination": "'' + deleted.Destination + ''" }'',
		deleted.State,
		deleted.Time
	into [' + @endpointSchema + '].[' + @endpointName + '.' + @dalayedTableSuffix + ']
';
exec(@migrateScript);

-- endcode
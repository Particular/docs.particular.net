-- startcode MigrateFromSql

declare @endpointName nvarchar(max) = N'PUT ENDPOINT NAME HERE';

declare @endpointSchema nvarchar(max) = N'dbo';
declare @dalayedTableSuffix nvarchar(max) = N'Delayed';
declare @timeoutDataTable nvarchar(max) = REPLACE(@endpointName,'.','_') + '_TimeoutData';

declare @migrateScript nvarchar(max);
set @migrateScript = N'
    WITH message AS (
		SELECT * 
		FROM ' + @timeoutDataTable + ' WITH (UPDLOCK, READPAST, ROWLOCK) 
		WHERE [Time] IS NOT NULL)
	DELETE FROM message
	OUTPUT
		LEFT(deleted.Headers, LEN(deleted.Headers) - 1) + '', "NServiceBus.SqlServer.ForwardDestination": "'' + deleted.Destination + ''" }'',
		deleted.State,
		deleted.Time
	INTO [' + @endpointSchema + '].[' + @endpointName + '.' + @dalayedTableSuffix + ']
';
exec(@migrateScript);

-- endcode
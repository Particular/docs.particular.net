startcode MsSqlServer_OutboxCleanupSql
WHILE 1 = 1
BEGIN
	DELETE TOP (@BatchSize) FROM [dbo].[EndpointNameOutboxData]
	WHERE Dispatched = 'true' AND DispatchedAt < @DispatchedBefore;
	IF @@ROWCOUNT < @BatchSize -- Important that @@ROWCOUNT is read immediately after the DELETE
		BREAK
END
endcode

startcode MsSqlServer_OutboxCleanupSql
WHILE 1 = 1
BEGIN
	-- Avoid batch sizes over 4.000 to prevent lock escalation
	DELETE TOP (@BatchSize) FROM [dbo].[EndpointNameOutboxData] WITH (ROWLOCK)
	WHERE Dispatched = 'true' AND DispatchedAt < @DispatchedBefore;
	IF @@ROWCOUNT < @BatchSize -- Important that @@ROWCOUNT is read immediately after the DELETE
		BREAK
END
endcode

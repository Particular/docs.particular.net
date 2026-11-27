startcode MsSqlServer_OutboxCleanupSql

delete top (@BatchSize) from [dbo].[EndpointNameOutboxData] with (rowlock, readpast)
where Dispatched = 1 and
      DispatchedAt < @DispatchedBefore
option (maxdop 1)
endcode
